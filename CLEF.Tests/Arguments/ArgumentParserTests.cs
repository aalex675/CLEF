using System;
using System.Linq;
using CLEF.Arguments;
using CLEF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Arguments
{
    [TestClass]
    public class ArgumentParserTests : ArgumentParserTestBase
    {
        private IArgumentParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new DefaultArgumentParser();
        }

        [TestMethod]
        public void Command()
        {
            var arg = this.parser.GetArguments(new string[] { "command" }).Single();

            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "command"), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Hyphen()
        {
            var arg = this.parser.GetArguments(new string[] { "-test" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, null), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Double_Hyphen()
        {
            var arg = this.parser.GetArguments(new string[] { "--test" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, null), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Slash()
        {
            var arg = this.parser.GetArguments(new string[] { "/test" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, null), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Equals_Separator()
        {
            var arg = this.parser.GetArguments(new string[] { "-test=\"Argument\"" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, "Argument"), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Colon_Separator()
        {
            var arg = this.parser.GetArguments(new string[] { "-test:\"Argument\"" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, "Argument"), arg);
        }

        [TestMethod]
        public void Unnamed_Parameter_String()
        {
            var arg = this.parser.GetArguments(new string[] { "\"Argument\"" }).Single();

            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "Argument"), arg);
        }

        [TestMethod]
        public void Command_With_Named_Parameter()
        {
            var args = this.parser.GetArguments(new string[] { "command", "-test=\"Argument\"" }).ToList();

            Assert.AreEqual(2, args.Count);
            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "command"), args[0]);
            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, "Argument"), args[1]);
        }

        [TestMethod]
        public void Command_With_Named_Parameter_No_Prefix()
        {
            var args = this.parser.GetArguments(new string[] { "command", "test=\"Argument\"" }).ToList();

            Assert.AreEqual(2, args.Count);
            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "command"), args[0]);
            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, "Argument"), args[1]);
        }

        [TestMethod]
        public void Command_With_Unnamed_Parameter()
        {
            var args = this.parser.GetArguments(new string[] { "command", "\"Argument\"" }).ToList();

            Assert.AreEqual(2, args.Count);
            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "command"), args[0]);
            this.AssertArgument(new Argument(null, ArgumentType.Unknown, "Argument"), args[1]);
        }
    }
}