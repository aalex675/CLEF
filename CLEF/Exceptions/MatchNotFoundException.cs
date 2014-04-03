using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Exceptions
{
    [Serializable]
    public class MatchNotFoundException : Exception
    {
        public MatchNotFoundException(string text)
            : base(string.Format("Could not find item matching the text '{0}'", text))
        {
            this.Text = text;
        }

        public string Text { get; set; }
    }
}