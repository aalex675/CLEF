using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Arguments
{
    public class Argument
    {
        public Argument(string name, ArgumentType type, string value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }

        public string Name { get; set; }

        public ArgumentType Type { get; set; }

        public string Value { get; set; }
    }
}