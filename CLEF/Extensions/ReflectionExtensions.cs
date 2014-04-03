using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    public static class ReflectionExtensions
    {
        public static bool IsBuiltInType(this Type type)
        {
            return type.Module.ScopeName == "CommonLanguageRuntimeLibrary";
        }
    }
}