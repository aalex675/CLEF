using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLEF.Browsers;
using CLEF.Commands;
using CLEF.Exceptions;
using CLEF.NameComparers;

namespace CLEF
{
    public static class CommandBrowserExtensions
    {
        public static bool CommandContainerExists(this IObjectBrowser finder, INameComparer matcher, Type type, object instance, string text)
        {
            var matchingContainers = finder.FindAllCommandContainers(type, instance).Where(c => c.IsMatch(matcher, text)).ToList();

            if (matchingContainers.Count == 1)
            {
                return true;
            }
            else if (matchingContainers.Count == 0)
            {
                return false;
            }
            else
            {
                throw new AmbiguousMatchException(text, matchingContainers.Select(c => c.Name).ToList());
            }
        }

        public static bool CommandExists(this IObjectBrowser finder, INameComparer matcher, Type type, object instance, string text)
        {
            var matchingCommands = finder.FindAllCommands(type, instance).Where(c => c.IsMatch(matcher, text)).ToList();

            if (matchingCommands.Count == 1)
            {
                return true;
            }
            else if (matchingCommands.Count == 0)
            {
                return false;
            }
            else
            {
                throw new AmbiguousMatchException(text, matchingCommands.Select(c => c.Name).ToList());
            }
        }

        public static ICommandContainer FindCommandContainer(this IObjectBrowser finder, INameComparer matcher, Type type, object instance, string text)
        {
            var matchingContainers = finder.FindAllCommandContainers(type, instance).Where(c => c.IsMatch(matcher, text)).ToList();

            if (matchingContainers.Count == 1)
            {
                return matchingContainers.First();
            }
            else if (matchingContainers.Count == 0)
            {
                throw new MatchNotFoundException(text);
            }
            else
            {
                throw new AmbiguousMatchException(text, matchingContainers.Select(c => c.Name).ToList());
            }
        }

        public static ICommand FindCommand(this IObjectBrowser finder, INameComparer matcher, Type type, object instance, string text)
        {
            var matchingCommands = finder.FindAllCommands(type, instance).Where(c => c.IsMatch(matcher, text)).ToList();

            if (matchingCommands.Count == 1)
            {
                return matchingCommands.First();
            }
            else if (matchingCommands.Count == 0)
            {
                throw new MatchNotFoundException(text);
            }
            else
            {
                throw new AmbiguousMatchException(text, matchingCommands.Select(c => c.Name).ToList());
            }
        }
    }
}