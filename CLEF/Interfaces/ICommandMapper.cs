using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;
using CLEF.Commands;

namespace CLEF
{
    public interface ICommandMapper
    {
        ICommand MapArgumentsToCommand(Type type, object instance, IList<Argument> argList);
    }
}