using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CLEF.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests.Commands
{
    [TestClass]
    public class ReflectionCommandTests
    {
        [TestMethod]
        public void Execute()
        {
            MethodInfo method = typeof(Test).GetMethods().First();

            ReflectionCommand command = new ReflectionCommand("Do", string.Empty, new List<IOption>(), new object[0], new Test(), method);

            command.Execute();
        }

        private class Test
        {
            public void Do()
            {
            }
        }
    }
}