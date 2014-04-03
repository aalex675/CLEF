using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CommandContainerAttribute : Attribute
    {
        public CommandContainerAttribute(string name, string description, params string[] alternateNames)
        {
            this.Name = name;
            this.Description = description;
            this.AlternateNames = alternateNames;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public string[] AlternateNames { get; set; }
    }
}