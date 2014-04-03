using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CLEF.Exceptions;
using CLEF.NameComparers;

namespace CLEF
{
    public static class NamedItemExtensions
    {
        public static bool IsMatch(this INamedItem item, INameComparer matcher, string text)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (matcher == null)
            {
                throw new ArgumentNullException("matcher");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            bool isMatch = false;

            if (item.Name != null && matcher.AreEqual(item.Name, text))
            {
                isMatch = true;
            }
            else if (item.AlternateNames != null && item.AlternateNames.Count(n => matcher.AreEqual(n, text)) > 0)
            {
                isMatch = true;
            }

            return isMatch;
        }

        public static T FindMatchingItem<T>(this IEnumerable<T> items, INameComparer matcher, string searchText)
            where T : INamedItem
        {
            var matches = items.Where(i => i.IsMatch(matcher, searchText)).ToList();

            if (matches.Count == 1)
            {
                return matches[0];
            }
            else if (matches.Count == 0)
            {
                throw new MatchNotFoundException(searchText);
            }
            else
            {
                throw new AmbiguousMatchException(searchText, matches.Select(m => m.Name).ToList());
            }
        }
    }
}