using System;
using CLEF;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;

namespace AttributeExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "Math", "Add", "-1=100", "-2=50" };
            }

            IRunner runner = new Runner(
                new DefaultArgumentParser(),
                new CommandMapper(
                    new AttributeObjectBrowser(),
                    new NameStartsWith(StringComparison.InvariantCultureIgnoreCase),
                    new DefaultHelpPrinter(15, "Attribute Example", new Version(1, 0)),
                    "?"));

            var context = new Context();
            runner.Execute(context, args);

            Console.ReadKey(true);
        }

        public class Context
        {
            public Context()
            {
                this.Math = new NestedContext();
            }

            [VerbContainer("Math", "Mathematical Operations")]
            public NestedContext Math { get; set; }

            public class NestedContext
            {
                [Verb("Add", "Adds two integers")]
                public void Add(
                    [Option("Value1", "The First Value", "v1", "1")]
                    int first,
                    [Option("Value2", "The Second Value", "v2", "2")]
                    int second)
                {
                    Console.WriteLine("{0} + {1} = {2}", first, second, first + second);
                }
            }
        }
    }
}