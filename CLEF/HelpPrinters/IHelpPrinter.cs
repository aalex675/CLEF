using System.Collections.Generic;
using CLEF.Arguments;

namespace CLEF.HelpPrinters
{
    public interface IHelpPrinter
    {
        void PrintHelp(IList<Argument> arguments, HelpContent content);
    }
}