using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;

namespace CLEF.Parsers
{
    public class ArgumentParserDefault : IArgumentParser
    {
        public readonly string[] ArgumentPrefixes = new string[] { "--", "-", "/" };
        public readonly string[] ArgumentValueSeparators = new string[] { "=", ":" };

        public IEnumerable<Argument> GetArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (this.IsNamedParameter(args[i]))
                {
                    string text = this.CleanName(this.GetNamedParameterName(args[i]));

                    if (this.NamedParameterHasValue(args[i]))
                    {
                        string value = this.CleanValue(this.GetNamedParameterValue(args[i]));
                        yield return new Argument(text, ArgumentType.NamedParameter, value);
                    }
                    else
                    {
                        yield return new Argument(text, ArgumentType.NamedParameter, null);
                    }
                }
                else
                {
                    // Unnamed parameter
                    string value = this.CleanValue(args[i]);
                    yield return new Argument(null, ArgumentType.Unknown, value);
                }
            }
        }

        private bool IsNamedParameter(string arg)
        {
            foreach (string prefix in this.ArgumentPrefixes)
            {
                if (arg.StartsWith(prefix))
                {
                    return true;
                }
            }

            if (this.GetNamedParameterParts(arg).Length == 2)
            {
                return true;
            }

            return false;
        }

        private bool NamedParameterHasValue(string arg)
        {
            string[] parts = this.GetNamedParameterParts(arg);
            if (parts.Length == 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private string[] GetNamedParameterParts(string arg)
        {
            return arg.Split(this.ArgumentValueSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        private string GetNamedParameterName(string arg)
        {
            return this.GetNamedParameterParts(arg)[0];
        }

        private string GetNamedParameterValue(string arg)
        {
            return this.GetNamedParameterParts(arg)[1];
        }

        private string CleanName(string arg)
        {
            string name = arg;
            foreach (string prefix in this.ArgumentPrefixes)
            {
                if (name.StartsWith(prefix))
                {
                    name = name.Substring(prefix.Length);
                    break;
                }
            }

            return name;
        }

        private string CleanValue(string arg)
        {
            string value = arg.Trim('"');

            return value;
        }
    }
}