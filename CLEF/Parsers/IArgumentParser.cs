using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Arguments;

namespace CLEF.Parsers
{
    public interface IArgumentParser
    {
        IEnumerable<Argument> GetArguments(string[] args);
    }
}