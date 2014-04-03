using System;
using CLEF;

namespace TestApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            args = new string[] { "SayHello", "-name=World" };

            CLEF.Browsers.IObjectBrowser browser = new CLEF.Browsers.ReflectionObjectBrowser();
            CLEF.NameComparers.INameComparer comparer = new CLEF.NameComparers.NameEquals(StringComparison.CurrentCulture);
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