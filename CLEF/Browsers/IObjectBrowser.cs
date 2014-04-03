using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Commands;

namespace CLEF.Browsers
{
    public interface IObjectBrowser
    {
        IEnumerable<ICommandContainer> FindAllCommandContainers(Type type, object instance);

        IEnumerable<IOption> FindGlobalOptions(Type type, object instance);

        IEnumerable<ICommand> FindAllCommands(Type type, object instance);
    }
}