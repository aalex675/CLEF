using System;
using CLEF.NameComparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Matching
{
    [TestClass]
    public class StartsWithNameMatcherTests
    {
        private NameStartsWith matcher;

        [TestInitialize]
        public void Initialize()
        {
            this.matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);
        }

        [TestMethod]
        public void Matches_Exact_Text()
        {
            Assert.IsTrue(this.matcher.AreEqual("Text", "Text"));
        }

        [TestMethod]
        public void Matches_Substring_1()
        {
            Assert.IsTrue(this.matcher.AreEqual("Text", "T"));
        }

        [TestMethod]
        public void Matches_Substring_2()
        {
            Assert.IsTrue(this.matcher.AreEqual("Text", "Te"));
        }

        [TestMethod]
        public void Matches_Case_Insensitive()
        {
            Assert.IsTrue(this.matcher.AreEqual("Text", "text"));
        }

        [TestMethod]
        public void Doesnt_Match_Different_Text_None()
        {
            Assert.IsFalse(this.matcher.AreEqual("Text", "None"));
        }

        [TestMethod]
        public void Doesnt_Match_Different_Text2()
        {
            Assert.IsFalse(this.matcher.AreEqual("Text", "Text2"));
        }
    }
}