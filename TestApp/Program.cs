using System;
using CLEF;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;

namespace TestApp
{
    public class Program
    {
        public static int Main(string[] args)
        {
            int number = 1;
            foreach (string arg in args)
            {
                Console.WriteLine("#{0}: {1}", number++, arg);
            }

            CommandMapper mapper = new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), new DefaultHelpPrinter(12, "Test Application", new Version(1, 2, 3, 4)), new string[] { "?" });
            IRunner runner = new Runner(new ArgumentParserStandard(), mapper);
            ExecutionContext context = new ExecutionContext();
            int result = runner.Execute<ExecutionContext>(context, new string[] { "?" });

            Console.ReadKey(true);

            return result;
        }

        public class ExecutionContext
        {
            public ExecutionContext()
            {
                this.Greet = new NestedContext();
            }

            public bool Test { get; set; }

            public string String { get; set; }

            public NestedContext Greet { get; set; }

            public void SayHello(string name, bool isCool)
            {
                if (this.Test)
                {
                    Console.WriteLine("Test");
                }

                Console.WriteLine(this.String);

                Console.WriteLine("Hello " + name);
                if (isCool)
                {
                    Console.WriteLine("You're cool!");
                }
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