using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    public interface INamedItem
    {
        string Name { get; }

        IList<string> AlternateNames { get; }
    }
}