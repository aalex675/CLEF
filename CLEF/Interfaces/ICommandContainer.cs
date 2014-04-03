using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    public interface ICommandContainer : INamedItem
    {
        string Description { get; }

        Type Type { get; }

        object Instance { get; }
    }
}