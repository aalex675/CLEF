using System;
using CLEF;

namespace HelloWorld
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "SayHello", "-name=World" };
            }

            IRunner runner = new Runner();

            ExecutionContext context = new ExecutionContext();
            int result = runner.Execute<ExecutionContext>(context, args);

            Console.ReadKey(true);

            return result;
        }

        public class ExecutionContext
        {
            public void SayHello(string name)
            {
                Console.WriteLine("Hello " + name);
            }
        }
    }
}