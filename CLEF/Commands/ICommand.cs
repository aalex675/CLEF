using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Commands
{
    public interface ICommand : INamedItem
    {
        string Description { get; }

        IList<IOption> Options { get; }

        object Execute();
    }
}