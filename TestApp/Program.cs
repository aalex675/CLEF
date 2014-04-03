﻿using System;
using CLEF;

namespace TestApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            args = new string[] { "SayHello", "-name=World" };

            // The IObjectBrowser tells the CommandMapper how to find Commands, CommandContainers (Verbs), and Global Options.
            CLEF.Browsers.IObjectBrowser browser = new CLEF.Browsers.ReflectionObjectBrowser();

            // The INameComparer tells the CommandMapper how to determine whether a command line argument is equal to a Command, CommandContainer, or Global Option.
            CLEF.NameComparers.INameComparer comparer = new CLEF.NameComparers.NameEquals(StringComparison.CurrentCulture);

            // The IHelpPrinter prints the HelpContent when the help command is called.
            CLEF.HelpPrinters.IHelpPrinter helpPrinter = new CLEF.HelpPrinters.DefaultHelpPrinter(15, "Application", new Version(1, 0));

            // The IArgumentParser is responsible for parsing the command line arguments into Argument objects.
            CLEF.Parsers.IArgumentParser parser = new CLEF.Parsers.DefaultArgumentParser();

            CommandMapper mapper = new CommandMapper(browser, comparer, helpPrinter, new string[] { "?" });
            IRunner runner = new Runner(parser, mapper);

            ExecutionContext context = new ExecutionContext();
            int result = runner.Execute<ExecutionContext>(context, args);

            Console.ReadKey(true);

            return result;
        }

        public class ExecutionContext
        {
            public void SayHello(string name)
            {
                Console.WriteLine("Hello " + name);
            }
        }
    }
}