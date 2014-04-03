using System;
using System.Collections.Generic;
using CLEF.Exceptions;
using CLEF.NameComparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CLEF.Tests
{
    [TestClass]
    public class NamedItemExtensionTests
    {
        private INamedItem item;
        private INameComparer alwaysMatches;
        private INameComparer matchesSameText;

        [TestInitialize]
        public void Initialize()
        {
            this.item = new Mock<INamedItem>().Object;
            var alwaysMatches = new Mock<INameComparer>();
            alwaysMatches.Setup(_ => _.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            this.alwaysMatches = alwaysMatches.Object;

            var matchesSame = new Mock<INameComparer>();
            matchesSame.Setup(_ => _.AreEqual(It.IsAny<string>(), It.IsAny<string>())).Returns((string v1, string v2) => v1 == v2);
            this.matchesSameText = matchesSame.Object;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsMatch_Throws_ArgumentNullException_When_INamedItem_Is_Null()
        {
            NamedItemExtensions.IsMatch(null, this.alwaysMatches, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsMatch_Throws_ArgumentNullException_When_INameMatcher_Is_Null()
        {
            NamedItemExtensions.IsMatch(this.item, null, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsMatch_Throws_ArgumentNullException_When_Text_Is_Null()
        {
            NamedItemExtensions.IsMatch(this.item, this.alwaysMatches, null);
        }

        [TestMethod]
        public void Matches_Item_Name()
        {
            var item = new Mock<INamedItem>();
            item.SetupGet(_ => _.Name).Returns("Test");
            Assert.IsTrue(NamedItemExtensions.IsMatch(item.Object, this.matchesSameText, "Test"));
        }

        [TestMethod]
        public void Matches_Alternate_Names()
        {
            var item = new Mock<INamedItem>();
            item.SetupGet(_ => _.Name).Returns("NoMatch");
            item.SetupGet(_ => _.AlternateNames).Returns(new List<string>() { "Test" });

            Assert.IsTrue(NamedItemExtensions.IsMatch(item.Object, this.matchesSameText, "Test"));
        }

        [TestMethod]
        public void Doesnt_Match()
        {
            Assert.IsFalse(NamedItemExtensions.IsMatch(this.item, this.matchesSameText, "Test"));
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void FindMatchingItem_Throws_MatchNotFoundException()
        {
            NamedItemExtensions.FindMatchingItem(new List<INamedItem>(), this.alwaysMatches, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void FindMatchingItem_Throws_AmbiguousMatchException()
        {
            var item = new Mock<INamedItem>();
            item.SetupGet(_ => _.Name).Returns("Text");

            NamedItemExtensions.FindMatchingItem(new List<INamedItem>() { item.Object, item.Object }, this.alwaysMatches, "Text");
        }
    }
}