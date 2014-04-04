CLEF
====

Command Line Execution Framework is a command line parsing library. It's purpose is to map command line arguments to functions.

Hello World Example
```C#
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
```
Console Usage:
```
>HelloWorld.exe SayHello "World"
Hello World
```

Architecture
===
- **IRunner** - Runs the arguments against the specified object instance or Type in the case of static methods.
    - **Runner***(Default)*
        - Dependencies:
            - **ICommandMapper** - Maps the arguments to the correct command and parameters on the object instance.
                - **CommandMapper***(Default)*
                    - Dependencies:
                        - **IObjectBrowser** - Browses the object type for Commands (Verbs), Command Containers, and Global Options.
                            - **AttributeObjectBrowser** - Uses VerbAttribute, VerbContainerAttribute, and Option attributes for discovery.
                            - **ReflectionObjectBrowser***(Default)* - Uses Reflection for discovery. Public methods become Verbs, Public Properties of Non-System Types are Command Containers, and Public Properties of System Types are Global Options.
                        - **INameComparer** - Compares argument names with Verb, Verb Container, and Option names to determine matches.
                            - **NameEquals** - Determines if argument names are equal to Command, Container, or Option names.
                            - **NameStartsWith***(Default)* - Determines if Command, Container, or Option names start with the argument name.
                        - **IHelpPrinter** - Prints the Help text if the user passes in a help command prefix.
                            - **DefaultHelpPrinter***(Default)* - Prints the help information to the console.
            - **IArgumentParser** - Parses the command line arguments into Argument objects.
                - **DefaultArgumentParser***(Default)* - Any dash or forward slash is a prefix for an argument name. '=' or ':' are valid value separators.
                - **StandardArgumentParser** - A single dash is a prefix for a single character shortname. A double dash is a prefix for a long name. Multiple shortnames can be joined together (Ex. '-test' is the same as '-t -e -s -t')

Here is a more complex example that utilizes nested verbs and global options:
```C#
using System;
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
```
Console Usage:
```
>ContextsAndGlobalOptions.exe Greet Hello -name=World -Title=Mr.
Hello Mr. World
>ContextsAndGlobalOptions.exe g hello -n=World -t=Mr.
Hello Mr. World
```
The second call works because the default INameMatcher is an instance of NameStartsWith that is case insensitive so 'g' matches 'Greet' and '-t' matches 'Title'.