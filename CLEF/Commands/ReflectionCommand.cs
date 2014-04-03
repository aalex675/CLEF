using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CLEF.Commands
{
    public class ReflectionCommand : ICommand
    {
        private object instance;
        private MethodInfo method;
        private object[] optionValues;

        public ReflectionCommand(string name, string description, IList<IOption> options, object[] optionValues, object instance, MethodInfo method, params string[] alternateNames)
        {
            this.Name = name;
            this.Description = description;
            this.Options = options;
            this.optionValues = optionValues;
            this.instance = instance;
            this.method = method;
            this.AlternateNames = alternateNames;
        }

        public string Name { get; protected set; }

        public IList<string> AlternateNames { get; set; }

        public string Description { get; protected set; }

        public IList<IOption> Options { get; protected set; }

        public object Execute()
        {
            return this.method.Invoke(this.instance, this.optionValues);
        }
    }
}