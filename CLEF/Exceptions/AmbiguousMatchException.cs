using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLEF.Exceptions
{
    [Serializable]
    public class AmbiguousMatchException : Exception
    {
        public AmbiguousMatchException(string text, IList<string> matchingCommands)
            : base(string.Format("Multiple matches were found for the text '{0}'. Items: {1}", text, string.Join(", ", matchingCommands)))
        {
            this.Text = text;
            this.MatchingItems = matchingCommands;
        }

        public string Text { get; set; }

        public IList<string> MatchingItems { get; set; }
    }
}