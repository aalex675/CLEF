CLEF
====

Command Line Execution Framework is a command line parsing library. Its purpose is to map command line arguments to functions.

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
    - **Runner** *(Default)*
        - Dependencies:
            - **ICommandMapper** - Maps the arguments to the correct command and parameters on the object instance.
                - **CommandMapper** *(Default)*
                    - Dependencies:
                        - **IObjectBrowser** - Browses the object type for Commands (Verbs), Command Containers, and Global Options.
                            - **AttributeObjectBrowser** - Uses VerbAttribute, VerbContainerAttribute, and OptionAttribute attributes for discovery.
                            - **ReflectionObjectBrowser** *(Default)* - Uses Reflection for discovery.
								- Public Methods = Verbs
								- Public Properties of non-system types = Verb Containers
								- Public Properties of system types = Global Options
                        - **INameComparer** - Compares argument names with Verb, Verb Container, and Option names to determine matches.
                            - **NameEquals** - Determines if argument names are equal to Command, Container, or Option names.
                            - **NameStartsWith** *(Default)* - Determines if Command, Container, or Option names start with the argument name.
                        - **IHelpPrinter** - Prints the Help text if the user passes in a help command prefix.
                            - **DefaultHelpPrinter** *(Default)* - Prints the help information to the console.
            - **IArgumentParser** - Parses the command line arguments into Argument objects.
                - **DefaultArgumentParser** *(Default)* - Any dash or forward slash is a prefix for an argument name. '=' or ':' are valid value separators.
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
>ContextsAndGlobalOptions.exe g hello -n=World
Hello World
```
The second call works because the default INameMatcher is an instance of NameStartsWith that is case insensitive so 'g' matches 'Greet' and '-t' matches 'Title'.

Example using attributes
```C#
using System;
using CLEF;
using CLEF.Browsers;
using CLEF.HelpPrinters;
using CLEF.NameComparers;
using CLEF.Parsers;

namespace AttributeExample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRunner runner = new Runner(
                new DefaultArgumentParser(),
                new CommandMapper(
                    new AttributeObjectBrowser(),
                    new NameStartsWith(StringComparison.InvariantCultureIgnoreCase),
                    new DefaultHelpPrinter(15, "Attribute Example", new Version(1, 0)),
                    "?"));

            var context = new Context();
            runner.Execute(context, args);

            Console.ReadKey(true);
        }

        public class Context
        {
            public Context()
            {
                this.Math = new NestedContext();
            }

            [VerbContainer("Math", "Mathematical Operations")]
            public NestedContext Math { get; set; }

            public class NestedContext
            {
                [Verb("Add", "Adds two integers")]
                public void Add(
                    [Option("Value1", "The First Value", "v1", "1")]
                    int first,
                    [Option("Value2", "The Second Value", "v2", "2")]
                    int second)
                {
                    Console.WriteLine("{0} + {1} = {2}", first, second, first + second);
                }
            }
        }
    }
}
```
Console Usage:
```
>AttributeExample.exe Math Add -1=100 -2=50
100 + 50 = 150
>AttributeExample.exe Math Add 100 50
100 + 50 = 150
```
