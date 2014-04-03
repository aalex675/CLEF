using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CLEF.Tests
{
    public class ConsoleOutputTester : IDisposable
    {
        private StringWriter writer;
        private string expectedOutput;
        private TextWriter originalOutput;

        public ConsoleOutputTester(string expectedOutput)
        {
            this.expectedOutput = expectedOutput;
            this.writer = new StringWriter();
            this.originalOutput = Console.Out;
            Console.SetOut(this.writer);
        }

        public void Dispose()
        {
            if (this.writer != null)
            {
                string writtenText = this.writer.ToString();
                Console.SetOut(this.originalOutput);
                Console.Write(writtenText);

                Assert.AreEqual(this.expectedOutput, writtenText);
                
                this.writer.Dispose();
                this.writer = null;
            }
        }
    }
}