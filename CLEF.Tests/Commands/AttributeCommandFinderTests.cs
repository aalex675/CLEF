using System;
using System.Linq;
using CLEF.Browsers;
using CLEF.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Commands
{
    [TestClass]
    public class AttributeCommandFinderTests
    {
        private IObjectBrowser finder;

        [TestInitialize]
        public void Initialize()
        {
            this.finder = new AttributeObjectBrowser();
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
                this.DoA = new Nested();
            }

            [Option("Global", "Global Option that does something")]
            public bool GlobalA { get; set; }

            [CommandContainer("Do", "Container for Do Commands")]
            public Nested DoA { get; set; }

            [Command("Go", "Does Something")]
            public void GoA()
            {
            }
        }

        private class InheritedSimple : Simple
        {
            public InheritedSimple()
            {
                this.DoA2 = new Nested();
            }

            [Option("Global2", "Global Option that does something")]
            public bool GlobalA2 { get; set; }

            [CommandContainer("Do2", "Container for Do2 Commands")]
            public Nested DoA2 { get; set; }

            [Command("Go2", "Does Something Else")]
            public void GoA2()
            {
            }
        }

        private class Nested
        {
            [Command("Test", "Test Method with option")]
            public void TestA(
                [Option("option1", "Int option")]
                int optionA1)
            {
            }
        }

        private class StaticSimple
        {
            [Option("Global", "Global Option that does something")]
            public static bool GlobalA { get; set; }

            [CommandContainer("Do", "Container for Do Commands")]
            public static StaticNested DoA { get; set; }

            [Command("Go", "Does Something")]
            public static void GoA()
            {
            }
        }

        private class StaticNested
        {
            [Command("Test", "Test Method with option")]
            public static void TestA(
                [Option("option1", "Int option")]
                int optionA1)
            {
            }
        }
    }
}