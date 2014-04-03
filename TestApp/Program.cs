using System;
using CLEF;

namespace TestApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            args = new string[] { "SayHello", "World" };

            CLEF.Browsers.IObjectBrowser browser = new CLEF.Browsers.ReflectionObjectBrowser();
            CLEF.NameComparers.INameComparer comparer = new CLEF.NameComparers.NameStartsWith(StringComparison.InvariantCultureIgnoreCase);
            CLEF.HelpPrinters.IHelpPrinter helpPrinter = new CLEF.HelpPrinters.DefaultHelpPrinter(15, "Application", new Version(1, 0));
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
            public ExecutionContext()
            {
                this.Greet = new NestedContext();
            }

            public bool IsTest { get; set; }

            public NestedContext Greet { get; set; }

            public void SayHello(string name)
            {
                if (this.IsTest)
                {
                    Console.WriteLine("Test");
                }

                Console.WriteLine("Hello " + name);
            }
        }

        public class NestedContext
        {
            public void Hey(string name)
            {
                Console.WriteLine("Hey " + name);
            }
        }
    }
}