using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Commands
{
    public class RelayOption : IOption
    {
        private Action<object> setValue;

        public RelayOption(string name, IList<string> alternateNames, string description, Type valueType, bool isRequired, object defaultValue, Action<object> setValue)
        {
            this.Name = name;
            this.AlternateNames = alternateNames;
            this.Description = description;
            this.ValueType = valueType;
            this.IsRequired = isRequired;
            this.DefaultValue = defaultValue;
            this.setValue = setValue;
        }

        public string Name { get; private set; }

        public IList<string> AlternateNames { get; private set; }

        public string Description { get; private set; }

        public Type ValueType { get; private set; }

        public bool IsRequired { get; private set; }

        public object DefaultValue { get; private set; }

        public bool HasUserValue { get; private set; }

        public void SetUserValue(object value)
        {
            this.setValue(value);
            this.HasUserValue = true;
        }
    }
}