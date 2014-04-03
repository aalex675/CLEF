using System;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Help
{
    [TestClass]
    public class DefaultHelpPrinterTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            DefaultHelpPrinter printer = new DefaultHelpPrinter(15, "Test", new Version(1, 0));
            Runner runner = new Runner(new ArgumentParserDefault(), new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), printer, new string[] { "?" }));

            using (var console = new ConsoleOutputTester(@"Help for Test v1.0
Arguments:
  ?
  value1 = Test
  value2
------------------------------------------------------------
-Global        Int32     

Nested
  -Global      Int32     

  With1Arg     
    -value     String    

With1Arg       
  -arg         Int32     

"))
            {
                runner.Execute(new Simple(), new string[] { "?", "-value1=Test", "-value2" });
            }
        }

        public class Simple
        {
            public Simple()
            {
                this.Nested = new Nested();
            }

            public int Global { get; set; }

            public Nested Nested { get; set; }

            public void With1Arg(int arg)
            {
            }
        }

        public class Nested
        {
            public int Global { get; set; }

            public void With1Arg(string value)
            {
            }
        }
    }
}