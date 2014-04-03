using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    public interface IRunner
    {
        int Execute(Type type, object instance, string[] args);

        int Execute<T>(T instance, string[] args);

        int ExecuteStatic(Type type, string[] args);
    }
}