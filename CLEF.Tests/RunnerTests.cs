using System;
using System.Collections.Generic;
using CLEF.Arguments;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CLEF.Tests
{
    [TestClass]
    public class RunnerTests
    {
        private IRunner runner;
        private IRunner attributeRunner;

        [TestInitialize]
        public void Initialize()
        {
            this.runner = new Runner(new DefaultArgumentParser(), new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), new DefaultHelpPrinter(15, "Tests", new Version(1, 0)), new string[] { "?", "help" }));
            this.attributeRunner = new Runner(new DefaultArgumentParser(), new CommandMapper(new AttributeObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), new DefaultHelpPrinter(15, "Tests", new Version(1, 0)), new string[] { "?", "help" }));
        }

        [TestMethod]
        public void Execute_Method()
        {
            var context = new Mock<Simple>();

            var result = this.runner.Execute(context.Object, new string[] { "Go" });

            context.Verify(_ => _.Go(), Times.Once()); 
        }

        [TestMethod]
        public void Execute_Method_With_Unnamed_Arg()
        {
            var context = new Mock<Simple>();

            this.runner.Execute<Simple>(context.Object, new string[] { "With1Arg", "True" });

            context.Verify(_ => _.With1Arg(true), Times.Once());
        }

        [TestMethod]
        public void Execute_Method_With_Named_Arg()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "With1Arg", "-value=False" });

            context.Verify(_ => _.With1Arg(false), Times.Once());
        }

        [TestMethod]
        public void Execute_Attribute_Method_With_Named_Arg()
        {
            var context = new Mock<AttributeSimple>();

            this.attributeRunner.Execute(context.Object, new string[] { "With1Arg", "-value=False" });

            context.Verify(_ => _.With1Arg(false), Times.Once());
        }

        [TestMethod]
        public void Execute_Method_With_Named_Arg_Separate()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "With1Arg", "-value", "False" });

            context.Verify(_ => _.With1Arg(false), Times.Once());
        }

        [TestMethod]
        public void Execute_Method_With_Named_Arg_Boolean_NoValue()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "With1Arg", "-value" });

            context.Verify(_ => _.With1Arg(true), Times.Once());
        }

        [TestMethod]
        public void Execute_Method_With_Global_Arg()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "Go", "-Global=True" });

            context.Verify(_ => _.Go(), Times.Once());
            context.VerifySet(_ => _.Global = true, Times.Once());
        }

        [TestMethod]
        public void Execute_Attribute_Method_With_Global_Arg()
        {
            var context = new Mock<AttributeSimple>();

            this.attributeRunner.Execute(context.Object, new string[] { "Go", "-Global=True" });

            context.Verify(_ => _.Go(), Times.Once());
            context.VerifySet(_ => _.Global = true, Times.Once());
        }

        [TestMethod]
        public void Sets_GlobalOptions_After_Arguments()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "With1Arg", "True", "False" });

            context.Verify(_ => _.With1Arg(true), Times.Once());
            context.VerifySet(_ => _.Global = false, Times.Once());
        }

        [TestMethod]
        public void Out_Of_Order_Arguments()
        {
            var context = new Mock<Simple>();

            this.runner.Execute(context.Object, new string[] { "ManyArgs", "-value2=5", "-value1=Test", "-value3=True" });

            context.Verify(_ => _.ManyArgs("Test", 5, true), Times.Once());
        }

        [TestMethod]
        public void Returns_Value()
        {
            Simple context = new Simple();

            var result = this.runner.Execute(context, new string[] { "ReturnsValue", "-value=100" });

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Shows_Help()
        {
            Mock<IHelpPrinter> helpPrinter = new Mock<IHelpPrinter>();
            IRunner runner = new Runner(new DefaultArgumentParser(), new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), helpPrinter.Object, new string[] { "?", "Help" }));

            Simple context = new Simple();

            var result = runner.Execute(context, new string[] { "Help" });

            helpPrinter.Verify(_ => _.PrintHelp(It.IsAny<IList<Argument>>(), It.IsAny<HelpContent>()), Times.Once());
        }

        [TestMethod]
        public void Shows_Help_2()
        {
            Mock<IHelpPrinter> helpPrinter = new Mock<IHelpPrinter>();
            IRunner runner = new Runner(new DefaultArgumentParser(), new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), helpPrinter.Object, new string[] { "?", "Help" }));

            Simple context = new Simple();

            var result = runner.Execute(context, new string[] { "-?" });

            helpPrinter.Verify(_ => _.PrintHelp(It.IsAny<IList<Argument>>(), It.IsAny<HelpContent>()), Times.Once());
        }

        [TestMethod]
        public void Executes_Static_Method()
        {
            var context = new Mock<StaticSimple>();

            var result = this.runner.ExecuteStatic(typeof(StaticSimple), new string[] { "Return200" });

            Assert.AreEqual(200, result);
        }

        [TestMethod]
        public void Executes_Nested_Method()
        {
            Simple context = new Simple();
            var nestedContext = new Mock<Nested>();
            context.Nested = nestedContext.Object;

            this.runner.Execute(context, new string[] { "Nested", "Go" });

            nestedContext.Verify(_ => _.Go(), Times.Once());
        }

        [TestMethod]
        public void Returns_Error_Code_On_Exception()
        {
            Simple context = new Simple();

            var result = this.runner.Execute(context, new string[] { "ThrowsException" });

            Assert.AreEqual(-1, result);
        }

        public class Simple
        {
            public virtual bool Global { get; set; }

            public virtual Nested Nested { get; set; }

            public virtual void Go()
            {
            }

            public virtual void With1Arg(bool value)
            {
            }

            public virtual void With1StringArg(string value)
            {
            }

            public virtual void ManyArgs(string value1, int value2, bool value3)
            {
            }

            public void ThrowsException()
            {
                throw new Exception();
            }

            public int ReturnsValue(int value)
            {
                return value;
            }
        }

        public class AttributeSimple
        {
            [Option("Global", "Global Option")]
            public virtual bool Global { get; set; }

            [Command("With1Arg", "With 1 Argument")]
            public virtual void With1Arg(
                [Option("value", "Value")]
                bool value)
            {
            }

            [Command("Go", "Go Command")]
            public virtual void Go()
            {
            }
        }

        public class Nested
        {
            public virtual void Go()
            {
            }
        }

        public class StaticSimple
        {
            public static int Return200()
            {
                return 200;
            }
        }
    }
}