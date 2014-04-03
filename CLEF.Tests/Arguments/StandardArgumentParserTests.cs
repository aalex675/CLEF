using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;
using CLEF.Parsers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Arguments
{
    [TestClass]
    public class StandardArgumentParserTests : ArgumentParserTestBase
    {
        private IArgumentParser parser;

        [TestInitialize]
        public void Initialize()
        {
            this.parser = new ArgumentParserStandard();
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
            var arg = this.parser.GetArguments(new string[] { "-t" }).Single();

            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), arg);
        }

        [TestMethod]
        public void Named_Parameter_With_Value()
        {
            var arg = this.parser.GetArguments(new string[] { "-t=True" }).Single();

            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, "True"), arg);
        }

        [TestMethod]
        public void Long_Named_Parameter_With_Value()
        {
            var arg = this.parser.GetArguments(new string[] { "--LongName=True" }).Single();

            this.AssertArgument(new Argument("LongName", ArgumentType.NamedParameter, "True"), arg);
        }

        [TestMethod]
        public void Long_Named_Parameter_Without_Value()
        {
            var arg = this.parser.GetArguments(new string[] { "--LongName=" }).Single();

            this.AssertArgument(new Argument("LongName", ArgumentType.NamedParameter, null), arg);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Long_Named_Parameter_With_Too_Many_Values_Throws_Exception()
        {
            var arg = this.parser.GetArguments(new string[] { "--LongName=1=2" }).Single();
        }

        [TestMethod]
        public void Multiple_Named_Parameters_With_Hyphen()
        {
            var args = this.parser.GetArguments(new string[] { "-test" }).ToList();

            Assert.AreEqual(4, args.Count);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[0]);
            this.AssertArgument(new Argument("e", ArgumentType.NamedParameter, null), args[1]);
            this.AssertArgument(new Argument("s", ArgumentType.NamedParameter, null), args[2]);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[3]);
        }

        [TestMethod]
        public void Multiple_Named_Parameters_With_Hyphen2()
        {
            var args = this.parser.GetArguments(new string[] { "-t-e-s-t" }).ToList();

            Assert.AreEqual(4, args.Count);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[0]);
            this.AssertArgument(new Argument("e", ArgumentType.NamedParameter, null), args[1]);
            this.AssertArgument(new Argument("s", ArgumentType.NamedParameter, null), args[2]);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[3]);
        }

        [TestMethod]
        public void Multiple_Named_Parameters_With_Hyphen3()
        {
            var args = this.parser.GetArguments(new string[] { "-t", "-e", "-s", "-t" }).ToList();

            Assert.AreEqual(4, args.Count);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[0]);
            this.AssertArgument(new Argument("e", ArgumentType.NamedParameter, null), args[1]);
            this.AssertArgument(new Argument("s", ArgumentType.NamedParameter, null), args[2]);
            this.AssertArgument(new Argument("t", ArgumentType.NamedParameter, null), args[3]);
        }

        [TestMethod]
        public void Long_Named_Parameter_With_Double_Hyphen()
        {
            var arg = this.parser.GetArguments(new string[] { "--test" }).Single();

            this.AssertArgument(new Argument("test", ArgumentType.NamedParameter, null), arg);
        }
    }
}