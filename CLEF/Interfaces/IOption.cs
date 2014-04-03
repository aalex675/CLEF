using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    public interface IOption : INamedItem
    {
        string Description { get; }

        Type ValueType { get; }

        bool IsRequired { get; }

        object DefaultValue { get; }

        bool HasUserValue { get; }

        void SetUserValue(object value);
    }
}