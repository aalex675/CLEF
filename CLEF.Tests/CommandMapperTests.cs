using System;
using System.Linq;
using CLEF.Browsers;
using CLEF.Exceptions;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CLEF.Tests
{
    [TestClass]
    public class CommandMapperTests
    {
        private ICommandMapper mapper;
        private IArgumentParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.mapper = new CommandMapper(new ReflectionObjectBrowser(), new NameStartsWith(StringComparison.InvariantCultureIgnoreCase), new DefaultHelpPrinter(15, "Test", new Version(1, 0)), new string[] { "?", "Help" });
            this.parser = new ArgumentParserDefault();
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void Too_Many_Arguments()
        {
            var context = new Mock<Simple>();

            this.mapper.MapArgumentsToCommand(typeof(Simple), context.Object, this.parser.GetArguments(new string[] { "Go", "-value" }).ToList());
        }

        [TestMethod]
        [ExpectedException(typeof(OptionValueMissingException))]
        public void Execute_Method_With_Named_Arg_Fails_With_No_Value()
        {
            var context = new Mock<Simple>();

            this.mapper.MapArgumentsToCommand(typeof(Simple), context.Object, this.parser.GetArguments(new string[] { "With1StringArg", "-value" }).ToList());
        }

        public class Simple
        {
            public virtual void Go()
            {
            }

            public virtual void With1StringArg(string value)
            {
            }
        }
    }
}