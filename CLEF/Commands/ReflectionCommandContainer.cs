using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Commands
{
    public class ReflectionCommandContainer : ICommandContainer
    {
        public ReflectionCommandContainer(string name, IList<string> alternateNames, string description, Type type, object instance)
        {
            this.Name = name;
            this.AlternateNames = alternateNames;
            this.Description = description;
            this.Type = type;
            this.Instance = instance;
        }

        public string Name { get; set; }

        public IList<string> AlternateNames { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public object Instance { get; set; }
    }
}