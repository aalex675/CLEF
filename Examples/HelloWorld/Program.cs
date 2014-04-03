using System;
using CLEF;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;

namespace HelloWorld
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "SayHello", "-name=World" };
            }

            // The IObjectBrowser tells the CommandMapper how to find Verbs, VerbContainers, and Global Options.
            IObjectBrowser browser = new ReflectionObjectBrowser();

            // The INameComparer tells the CommandMapper how to determine whether a command line argument is equal to a Command, CommandContainer, or Global Option.
            INameComparer comparer = new NameEquals(StringComparison.CurrentCulture);

            // The IHelpPrinter prints the HelpContent when the help command is called.
            IHelpPrinter helpPrinter = new DefaultHelpPrinter(15, "Application", new Version(1, 0));

            // The IArgumentParser is responsible for parsing the command line arguments into Argument objects.
            IArgumentParser parser = new DefaultArgumentParser();

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