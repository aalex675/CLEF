using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Arguments
{
    public abstract class ArgumentParserTestBase
    {
        protected void AssertArgument(Argument expected, Argument actual)
        {
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.Value, actual.Value);
        }
    }
}