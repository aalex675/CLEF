using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.NameComparers
{
    public class NameEquals : INameComparer
    {
        private StringComparison comparison;

        public NameEquals(StringComparison comparison)
        {
            this.comparison = comparison;
        }

        public bool AreEqual(string name, string text)
        {
            return name.Equals(text, this.comparison);
        }
    }
}