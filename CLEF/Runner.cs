using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CLEF.Arguments;
using CLEF.Commands;
using CLEF.Parsers;

namespace CLEF
{
    public class Runner : IRunner
    {
        private IArgumentParser parser;
        private ICommandMapper mapper;

        public Runner()
        {
            this.parser = new DefaultArgumentParser();
            var currentProgramName = Assembly.GetEntryAssembly().GetName();
            this.mapper = new CommandMapper(
                new Browsers.ReflectionObjectBrowser(),
                new NameComparers.NameStartsWith(StringComparison.InvariantCultureIgnoreCase),
                new HelpPrinters.DefaultHelpPrinter(15, currentProgramName.Name, currentProgramName.Version),
                new string[] { "?", "Help" });
        }

        public Runner(IArgumentParser parser, ICommandMapper mapper)
        {
            this.parser = parser;
            this.mapper = mapper;
        }

        public int Execute(Type type, object instance, string[] args)
        {
            try
            {
                var argList = this.parser.GetArguments(args).ToList();

                ICommand command = this.mapper.MapArgumentsToCommand(type, instance, argList);

                object result = command.Execute();
                if (result is bool || result is byte || result is ushort || result is short || result is uint || result is int || result is long || result is ulong)
                {
                    return (int)result;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }

        public int Execute<T>(T instance, string[] args)
        {
            return this.Execute(typeof(T), instance, args);
        }

        public int ExecuteStatic(Type type, string[] args)
        {
            return this.Execute(type, null, args);
        }
    }
}