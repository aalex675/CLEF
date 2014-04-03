using System;
using System.Collections.Generic;
using CLEF.Browsers;
using CLEF.Commands;
using CLEF.Exceptions;
using CLEF.NameComparers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CLEF.Tests.Commands
{
    [TestClass]
    public class CommandFinderExtenstionTests
    {
        [TestMethod]
        public void CommandContainer_Exists()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                        new TestContainer() { Name = "TestContainer" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            bool exists = CommandBrowserExtensions.CommandContainerExists(finder.Object, matcher, typeof(object), new object(), "TestContainer");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void CommandContainer_Doesnt_Exist()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            bool exists = CommandBrowserExtensions.CommandContainerExists(finder.Object, matcher, typeof(object), new object(), "TestContainer");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void CommandContainerExists_Throws_AmbiguousMatchException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                        new TestContainer() { Name = "TestContainer" },
                        new TestContainer() { Name = "TestContainer" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            CommandBrowserExtensions.CommandContainerExists(finder.Object, matcher, typeof(object), new object(), "TestContainer");
        }

        [TestMethod]
        public void Command_Exists()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                        new TestCommand() { Name = "TestCommand" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            bool exists = CommandBrowserExtensions.CommandExists(finder.Object, matcher, typeof(object), new object(), "TestCommand");

            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void Command_Doesnt_Exist()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            bool exists = CommandBrowserExtensions.CommandExists(finder.Object, matcher, typeof(object), new object(), "TestCommand");

            Assert.IsFalse(exists);
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void CommandExists_Throws_AmbiguousMatchException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                        new TestCommand() { Name = "TestCommand" },
                        new TestCommand() { Name = "TestCommand" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            CommandBrowserExtensions.CommandExists(finder.Object, matcher, typeof(object), new object(), "TestCommand");
        }

        [TestMethod]
        public void FindCommand()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                        new TestCommand() { Name = "TestCommand" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommand(finder.Object, matcher, typeof(object), new object(), "TestCommand");

            Assert.AreEqual("TestCommand", command.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void FindCommand_Throws_MatchNotFoundException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommand(finder.Object, matcher, typeof(object), new object(), "TestCommand");
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void FindCommand_Throws_AmbiguousMatchException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommands(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommand>()
                    {
                        new TestCommand() { Name = "TestCommand" },
                        new TestCommand() { Name = "TestCommand" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommand(finder.Object, matcher, typeof(object), new object(), "TestCommand");
        }

        [TestMethod]
        public void FindCommandContainer()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                        new TestContainer() { Name = "TestContainer" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommandContainer(finder.Object, matcher, typeof(object), new object(), "TestContainer");

            Assert.AreEqual("TestContainer", command.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchNotFoundException))]
        public void FindCommandContainer_Throws_MatchNotFoundException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommandContainer(finder.Object, matcher, typeof(object), new object(), "TestContainer");
        }

        [TestMethod]
        [ExpectedException(typeof(AmbiguousMatchException))]
        public void FindCommandContainer_Throws_AmbiguousMatchException()
        {
            Mock<IObjectBrowser> finder = new Mock<IObjectBrowser>();
            finder.Setup(_ => _.FindAllCommandContainers(typeof(object), It.IsAny<object>())).Returns(
                () =>
                {
                    return new List<ICommandContainer>()
                    {
                        new TestContainer() { Name = "TestContainer" },
                        new TestContainer() { Name = "TestContainer" }
                    };
                });
            INameComparer matcher = new NameStartsWith(StringComparison.InvariantCultureIgnoreCase);

            var command = CommandBrowserExtensions.FindCommandContainer(finder.Object, matcher, typeof(object), new object(), "TestContainer");
        }

        private class TestContainer : ICommandContainer
        {
            public string Name { get; set; }

            public IList<string> AlternateNames { get; set; }

            public string Description { get; set; }

            public Type Type { get; set; }

            public object Instance { get; set; }
        }

        private class TestCommand : ICommand
        {
            public string Name { get; set; }

            public IList<string> AlternateNames { get; set; }

            public string Description { get; set; }

            public IList<IOption> Options { get; set; }

            public void SetGlobalParameter(string name, object value, object instance)
            {
                throw new NotImplementedException();
            }

            public object Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}