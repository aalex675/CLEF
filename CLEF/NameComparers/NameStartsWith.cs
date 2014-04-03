using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.NameComparers
{
    public class NameStartsWith : INameComparer
    {
        private StringComparison comparison;

        public NameStartsWith(StringComparison comparison)
        {
            this.comparison = comparison;
        }

        public bool AreEqual(string name, string text)
        {
            return name.StartsWith(text, this.comparison);
        }
    }
}