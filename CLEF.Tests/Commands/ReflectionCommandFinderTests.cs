using System;
using System.Linq;
using CLEF.Browsers;
using CLEF.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Commands
{
    [TestClass]
    public class ReflectionCommandFinderTests
    {
        private IObjectBrowser finder;

        [TestInitialize]
        public void Initialize()
        {
            this.finder = new ReflectionObjectBrowser();
        }

        [TestMethod]
        public void Finds_Command()
        {
            var commands = this.finder.FindAllCommands(typeof(Simple), new Simple()).ToList();

            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("Go", commands[0].Name);
        }

        [TestMethod]
        public void Finds_Command_Container()
        {
            var containers = this.finder.FindAllCommandContainers(typeof(Simple), new Simple()).ToList();

            Assert.AreEqual(1, containers.Count);
            Assert.AreEqual("Do", containers[0].Name);
        }

        [TestMethod]
        public void Finds_Global_Option()
        {
            var options = this.finder.FindGlobalOptions(typeof(Simple), new Simple()).ToList();

            Assert.AreEqual(1, options.Count);
            Assert.AreEqual("Global", options[0].Name);
        }

        [TestMethod]
        public void Finds_Command_Options()
        {
            var commands = this.finder.FindAllCommands(typeof(Nested), new Nested()).ToList();

            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual(1, commands[0].Options.Count);
            Assert.AreEqual("option1", commands[0].Options[0].Name);
        }

        [TestMethod]
        public void Finds_Inherited_Commands()
        {
            var commands = this.finder.FindAllCommands(typeof(InheritedSimple), new InheritedSimple()).ToList();

            Assert.AreEqual(2, commands.Count);
            Assert.AreEqual("Go2", commands[0].Name);
            Assert.AreEqual("Go", commands[1].Name);
        }

        [TestMethod]
        public void Finds_Inherited_Command_Containers()
        {
            var containers = this.finder.FindAllCommandContainers(typeof(InheritedSimple), new InheritedSimple()).ToList();

            Assert.AreEqual(2, containers.Count);
            Assert.AreEqual("Do2", containers[0].Name);
            Assert.AreEqual("Do", containers[1].Name);
        }

        [TestMethod]
        public void Finds_Inherited_Global_Options()
        {
            var options = this.finder.FindGlobalOptions(typeof(InheritedSimple), new InheritedSimple()).ToList();

            Assert.AreEqual(2, options.Count);
            Assert.AreEqual("Global2", options[0].Name);
            Assert.AreEqual("Global", options[1].Name);
        }

        [TestMethod]
        public void Finds_Static_Command()
        {
            var commands = this.finder.FindAllCommands(typeof(StaticSimple), null).ToList();

            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual("Go", commands[0].Name);
        }

        [TestMethod]
        public void Finds_Static_Command_Container()
        {
            var containers = this.finder.FindAllCommandContainers(typeof(StaticSimple), null).ToList();

            Assert.AreEqual(1, containers.Count);
            Assert.AreEqual("Do", containers[0].Name);
        }

        [TestMethod]
        public void Finds_Static_Global_Option()
        {
            var options = this.finder.FindGlobalOptions(typeof(StaticSimple), null).ToList();

            Assert.AreEqual(1, options.Count);
            Assert.AreEqual("Global", options[0].Name);
        }

        [TestMethod]
        public void Finds_Static_Command_Options()
        {
            var commands = this.finder.FindAllCommands(typeof(StaticNested), null).ToList();

            Assert.AreEqual(1, commands.Count);
            Assert.AreEqual(1, commands[0].Options.Count);
            Assert.AreEqual("option1", commands[0].Options[0].Name);
        }

        private class Simple
        {
            public Simple()
            {
                this.Do = new Nested();
            }

            public string Global { get; set; }

            public Nested Do { get; set; }

            public void Go()
            {
            }
        }

        private class InheritedSimple : Simple
        {
            public InheritedSimple()
            {
                this.Do2 = new Nested();
            }

            public string Global2 { get; set; }

            public Nested Do2 { get; set; }

            public void Go2()
            {
            }
        }

        private class Nested
        {
            public void Test(int option1)
            {
            }
        }

        private class StaticSimple
        {
            public static string Global { get; set; }

            public static StaticNested Do { get; set; }

            public static void Go()
            {
            }
        }

        private class StaticNested
        {
            public static void Test(int option1)
            {
            }
        }
    }
}