using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Exceptions
{
    public class OptionValueMissingException : Exception
    {
        public OptionValueMissingException(string optionName, Type valueType)
            : base(string.Format("Option '{0}' expected a value of type {1}, but the value was null", optionName, valueType))
        {
            this.OptionName = optionName;
            this.ValueType = valueType;
        }

        public string OptionName { get; set; }

        public Type ValueType { get; set; }
    }
}