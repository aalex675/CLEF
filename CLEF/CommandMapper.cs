using System;
using System.Collections.Generic;
using System.Linq;
using CLEF.Arguments;
using CLEF.Browsers;
using CLEF.Commands;
using CLEF.Exceptions;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using PSC.Common.Utilities;

namespace CLEF
{
    public class CommandMapper : ICommandMapper
    {
        private string[] helpCommandNames;

        private IObjectBrowser browser;
        private INameComparer comparer;
        private IHelpPrinter helpPrinter;

        public CommandMapper(IObjectBrowser browser, INameComparer comparer, IHelpPrinter helpPrinter, string[] helpCommandNames)
        {
            this.browser = browser;
            this.comparer = comparer;
            this.helpPrinter = helpPrinter;
            this.helpCommandNames = helpCommandNames;
        }

        public ICommand MapArgumentsToCommand(Type type, object instance, IList<Argument> argList)
        {
            int endIndex = 0;
            ICommandContainer context = this.FindContainer(type, instance, argList, out endIndex);
            ICommand command = this.FindCommand(context, argList, endIndex, out endIndex);
            this.ParseArguments(context, command, argList, endIndex);

            return command;
        }

        private ICommandContainer FindContainer(Type type, object instance, IList<Argument> argList, out int endIndex)
        {
            ICommandContainer context = new ReflectionCommandContainer(null, null, null, type, instance);

            int i = 0;
            for (i = 0; i < argList.Count; i++)
            {
                Argument arg = argList[i];

                if (arg.Value != null && this.browser.CommandContainerExists(this.comparer, context.Type, context.Instance, arg.Value))
                {
                    context = this.browser.FindCommandContainer(this.comparer, context.Type, context.Instance, arg.Value);
                }
                else
                {
                    break;
                }
            }

            endIndex = i;
            return context;
        }

        private ICommand FindCommand(ICommandContainer container, IList<Argument> argList, int startIndex, out int endIndex)
        {
            RelayCommand helpCommand = new RelayCommand(this.helpCommandNames.First(), this.helpCommandNames.Skip(1).ToList(), "Show Help", () => this.ShowHelp(argList, container.Type, container.Instance));

            ICommand command = helpCommand;

            Argument arg = argList[startIndex];

            if (this.IsHelpCommand(arg, container, helpCommand) == true)
            {
                // Do nothing
                startIndex = argList.Count - 1;
            }
            else if (this.browser.CommandExists(this.comparer, container.Type, container.Instance, arg.Value))
            {
                command = this.browser.FindCommand(this.comparer, container.Type, container.Instance, arg.Value);
            }

            endIndex = startIndex + 1;
            return command;
        }

        private bool IsHelpCommand(Argument arg, ICommandContainer context, RelayCommand helpCommand)
        {
            string value = arg.Value ?? arg.Name;

            if (helpCommand.IsMatch(this.comparer, value))
            {
                return true;
            }

            return false;
        }

        private void ParseArguments(ICommandContainer context, ICommand command, IList<Argument> argList, int startIndex)
        {
            var globalOptions = this.browser.FindGlobalOptions(context.Type, context.Instance).ToList();
            List<IOption> allOptions = command.Options.Union(globalOptions).ToList();

            for (int i = startIndex; i < argList.Count; i++)
            {
                Argument arg = argList[i];
                Argument nextArg = argList.Skip(i + 1).FirstOrDefault();

                bool skipNextArg = false;
                this.SetOptionValue(allOptions, arg, nextArg, out skipNextArg);
                if (skipNextArg)
                {
                    i++;
                }
            }
        }

        private void SetOptionValue(List<IOption> options, Argument argument, Argument nextArgument, out bool skipNextArgument)
        {
            var param = this.GetNextUnusedParameter(options);

            if (argument.Name != null)
            {
                if (param.IsMatch(this.comparer, argument.Name) == false)
                {
                    // Out of order option, check all options for a match
                    param = options.FindMatchingItem(this.comparer, argument.Name);
                }
            }

            // Try to convert the value
            // TODO: Throw exception with context information
            skipNextArgument = false;
            if (param.ValueType == typeof(bool) && argument.Value == null && (nextArgument == null || nextArgument.Name != null))
            {
                // If the parameter type is boolean, then just having the argument there means to enable that option
                param.SetUserValue(true);
            }
            else if (argument.Value != null)
            {
                param.SetUserValue(TConverter.ChangeType(param.ValueType, argument.Value));
            }
            else if (nextArgument != null && nextArgument.Name == null)
            {
                param.SetUserValue(TConverter.ChangeType(param.ValueType, nextArgument.Value));
                skipNextArgument = true;
            }
            else
            {
                throw new OptionValueMissingException(param.Name, param.ValueType);
            }
        }

        private IOption GetNextUnusedParameter(IEnumerable<IOption> options)
        {
            // Default to assuming the value is for the next option that hasn't been set yet
            var param = options.FirstOrDefault(o => o.HasUserValue == false);
            if (param == null)
            {
                throw new MatchNotFoundException(null);
            }

            return param;
        }

        private object ShowHelp(IList<Argument> arguments, Type type, object instance)
        {
            HelpContent content = this.GetContent(type, instance);

            this.helpPrinter.PrintHelp(arguments, content);

            return null;
        }

        private HelpContent GetContent(Type type, object instance)
        {
            HelpContent content = new HelpContent();
            content.Type = type;
            content.Instance = instance;

            content.GlobalOptions.AddRange(this.browser.FindGlobalOptions(type, instance));
            content.Commands.AddRange(this.browser.FindAllCommands(type, instance));
            foreach (var container in this.browser.FindAllCommandContainers(type, instance))
            {
                HelpContent subContent = this.GetContent(container.Type, container.Instance);
                subContent.Name = container.Name;
                subContent.AlternateNames = container.AlternateNames;
                subContent.Description = container.Description;

                content.Containers.Add(subContent);
            }

            return content;
        }

        private class RelayCommand : ICommand
        {
            private Func<object> function;

            public RelayCommand(string name, IList<string> alternateNames, string description, Func<object> function)
            {
                this.Name = name;
                this.AlternateNames = alternateNames;
                this.Description = description;
                this.Options = new List<IOption>();
                this.function = function;
            }

            public string Name { get; set; }

            public IList<string> AlternateNames { get; set; }

            public string Description { get; set; }

            public IList<IOption> Options { get; set; }

            public object Execute()
            {
                return this.function();
            }
        }
    }
}