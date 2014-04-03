using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.NameComparers
{
    public interface INameComparer
    {
        bool AreEqual(string name, string text);
    }
}