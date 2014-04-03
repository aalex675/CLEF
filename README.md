CLEF
====

Command Line Execution Framework is a command line parsing library. It's purpose is to map command line arguments to functions.

Hello World example:
```C#
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
            public void SayHello(string name)
            {
                Console.WriteLine("Hello " + name);
            }
        }
    }
}
```