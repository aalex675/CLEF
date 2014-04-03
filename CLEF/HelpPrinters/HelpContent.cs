using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Commands;

namespace CLEF.HelpPrinters
{
    public class HelpContent : ICommandContainer
    {
        public HelpContent()
        {
            this.GlobalOptions = new List<IOption>();
            this.Commands = new List<ICommand>();
            this.Containers = new List<HelpContent>();
        }

        public string Name { get; set; }

        public IList<string> AlternateNames { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public object Instance { get; set; }

        public List<IOption> GlobalOptions { get; private set; }

        public List<ICommand> Commands { get; private set; }

        public List<HelpContent> Containers { get; private set; }
    }
}