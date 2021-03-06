﻿using System;
using CLEF;

namespace ContextsAndGlobalOptions
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new string[] { "Greet", "Hello", "-name=World", "-Title=Mr." };
            }

            IRunner runner = new Runner();

            ComplexContext complexContext = new ComplexContext();
            int result = runner.Execute<ComplexContext>(complexContext, args);

            Console.ReadKey(true);

            return result;
        }

        public class ComplexContext
        {
            public ComplexContext()
            {
                this.Greet = new NestedContext();
            }

            public NestedContext Greet { get; set; }
        }

        public class NestedContext
        {
            public string Title { get; set; }

            public void Hello(string name)
            {
                Console.WriteLine("Hello " + this.GetNameWithTitle(name));
            }

            public void Hi(string name)
            {
                Console.WriteLine("Hi " + this.GetNameWithTitle(name));
            }

            private string GetNameWithTitle(string name)
            {
                if (this.Title != null)
                {
                    return string.Join(" ", this.Title, name);
                }
                else
                {
                    return name;
                }
            }
        }
    }
}