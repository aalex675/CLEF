using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;

namespace CLEF.Parsers
{
    public class StandardArgumentParser : IArgumentParser
    {
        private readonly char[] validDashes = { '-', '‐', '‒', '–', '—', '―' };
        private readonly char valueSeparator = '=';

        public IEnumerable<Argument> GetArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (this.StartsWithSingleDash(args[i]))
                {
                    string arg = this.TrimStartDashes(args[i]);
                    for (int c = 0; c < arg.Length; c++)
                    {
                        if (this.validDashes.Contains(arg[c]) == false)
                        {
                            string name = arg[c].ToString();
                            if (c + 1 < arg.Length && arg[c + 1] == this.valueSeparator)
                            {
                                string value = arg.Substring(c + 2);
                                yield return new Argument(name, ArgumentType.NamedParameter, this.CleanValue(value));
                                break;
                            }
                            else
                            {
                                yield return new Argument(name, ArgumentType.NamedParameter, null);
                            }
                        }
                    }
                }
                else if (this.StartsWithDoubleDash(args[i]))
                {
                    string arg = this.TrimStartDashes(args[i]);
                    if (arg.Contains(this.valueSeparator))
                    {
                        yield return this.ParseNamedArgument(arg);
                    }
                    else
                    {
                        yield return new Argument(arg, ArgumentType.NamedParameter, null);
                    }
                }
                else
                {
                    yield return new Argument(null, ArgumentType.Unknown, this.CleanValue(args[i]));
                }
            }
        }

        private Argument ParseNamedArgument(string arg)
        {
            string[] parts = arg.Split(new string[] { this.valueSeparator.ToString() }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1)
            {
                return new Argument(parts[0], ArgumentType.NamedParameter, null);
            }
            else if (parts.Length == 2)
            {
                return new Argument(parts[0], ArgumentType.NamedParameter, this.CleanValue(parts[1]));
            }
            else
            {
                throw new Exception("Crap");
            }
        }

        private bool StartsWithDoubleDash(string arg)
        {
            if (arg.Length > 1)
            {
                if (this.validDashes.Contains(arg[0]) &&
                    this.validDashes.Contains(arg[1]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool StartsWithSingleDash(string arg)
        {
            if (arg.Length > 1)
            {
                if (this.validDashes.Contains(arg[0]) &&
                    this.validDashes.Contains(arg[1]) == false)
                {
                    return true;
                }
            }

            return false;
        }

        private string TrimStartDashes(string arg)
        {
            return arg.TrimStart(this.validDashes);
        }

        private string CleanValue(string arg)
        {
            string value = arg.Trim('"');

            return value;
        }
    }
}