using System;
using System.Collections.Generic;
using System.Linq;
using CLEF.Arguments;
using CLEF.Commands;

namespace CLEF.HelpPrinters
{
    public class DefaultHelpPrinter : IHelpPrinter
    {
        private string applicationName;
        private Version version;
        private int firstColumnPadding;

        public DefaultHelpPrinter(int firstColumnPadding, string applicationName, Version version)
        {
            this.firstColumnPadding = firstColumnPadding;
            this.applicationName = applicationName;
            this.version = version;
        }

        public void PrintHelp(IList<Argument> arguments, HelpContent content)
        {
            this.ShowHelp(content, arguments, 0);
        }

        private void ShowHelp(HelpContent content, IList<Argument> arguments, int indentLevel = 0)
        {
            this.PrintHeader(arguments, indentLevel);
            this.PrintOptions(content.GlobalOptions, indentLevel);
            this.PrintContainers(content.Containers, indentLevel);
            this.PrintCommands(content.Commands, indentLevel);
        }

        private void PrintHeader(IList<Argument> arguments, int indentLevel)
        {
            if (indentLevel == 0)
            {
                Console.WriteLine(this.Indent(indentLevel, "Help for " + this.applicationName + " v" + this.version.ToString()));
                Console.WriteLine(this.Indent(indentLevel, "Arguments:"));
                foreach (var arg in arguments)
                {
                    Console.WriteLine(this.Indent(indentLevel + 1, this.GetArgValue(arg)));
                }

                Console.WriteLine(this.Indent(indentLevel, new string('-', 60)));
            }
        }

        private string GetArgValue(Argument arg)
        {
            if (arg.Name != null)
            {
                if (arg.Value != null)
                {
                    return arg.Name + " = " + arg.Value;
                }
                else
                {
                    return arg.Name;
                }
            }
            else
            {
                return arg.Value;
            }
        }

        private void PrintOptions(List<IOption> options, int indentLevel)
        {
            if (options.Count > 0)
            {
                foreach (var option in options)
                {
                    Console.WriteLine(this.Indent(indentLevel, "-" + option.Name).PadRight(this.firstColumnPadding) + option.ValueType.Name.PadRight(10) + option.Description);
                }

                Console.WriteLine();
            }
        }

        private void PrintContainers(List<HelpContent> containers, int indentLevel)
        {
            if (containers.Count > 0)
            {
                ////Console.WriteLine(this.Indent(indentLevel, "Command Containers:"));
                foreach (var container in containers)
                {
                    Console.WriteLine(this.Indent(indentLevel, container.Name));
                    this.ShowHelp(container, null, indentLevel + 1);
                }
            }
        }

        private void PrintCommands(List<ICommand> commands, int indentLevel)
        {
            if (commands.Count > 0)
            {
                ////Console.WriteLine(Indent(indentLevel, "Commands:"));
                foreach (var command in commands)
                {
                    Console.WriteLine(this.Indent(indentLevel, command.Name).PadRight(this.firstColumnPadding) + command.Description);
                    this.PrintOptions(command.Options.ToList(), indentLevel + 1);
                }
            }
        }

        private string Indent(int indentLevel, string text)
        {
            return new string(' ', indentLevel * 2) + text;
        }
    }
}