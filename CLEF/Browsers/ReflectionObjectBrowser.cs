using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CLEF.Commands;

namespace CLEF.Browsers
{
    /// <summary>
    /// Discovers Commands, Containers, and Global Options using reflection.
    /// </summary>
    public class ReflectionObjectBrowser : IObjectBrowser
    {
        public IEnumerable<ICommandContainer> FindAllCommandContainers(Type type, object instance)
        {
            PropertyInfo[] properties = null;

            if (instance == null)
            {
                properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            }
            else
            {
                properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            }

            foreach (var property in properties)
            {
                if (property.PropertyType.IsBuiltInType() == false)
                {
                    object propertyValue = property.GetValue(instance, null);
                    yield return new ReflectionCommandContainer(property.Name, new List<string>(), string.Empty, property.PropertyType, propertyValue);
                }
            }
        }

        public IEnumerable<IOption> FindGlobalOptions(Type type, object instance)
        {
            PropertyInfo[] properties = null;

            if (instance == null)
            {
                properties = type.GetProperties(BindingFlags.Static | BindingFlags.Public);
            }
            else
            {
                properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            }

            foreach (var property in properties)
            {
                if (property.PropertyType.IsBuiltInType() == true)
                {
                    var globalProperty = property;
                    object propertyValue = globalProperty.GetValue(instance, null);
                    yield return new RelayOption(property.Name, new List<string>(), string.Empty, property.PropertyType, true, null, newValue => globalProperty.SetValue(instance, newValue, null));
                }
            }
        }

        public IEnumerable<ICommand> FindAllCommands(Type type, object instance)
        {
            IEnumerable<MethodInfo> methods = null;

            if (instance == null)
            {
                methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(m => m.IsSpecialName == false);
            }
            else
            {
                methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(m => m.IsSpecialName == false);
            }

            foreach (var method in methods)
            {
                List<IOption> options = new List<IOption>();
                var parameters = method.GetParameters();
                object[] optionValues = new object[parameters.Count()];
                int i = 0;
                foreach (var p in parameters)
                {
                    int itemIndex = i;
                    options.Add(new RelayOption(p.Name, new List<string>(), string.Empty, p.ParameterType, !p.IsOptional, p.DefaultValue, newValue => optionValues[itemIndex] = newValue));
                    i++;
                }

                yield return new ReflectionCommand(method.Name, string.Empty, options, optionValues, instance, method);
            }

            // Recurse to the next type up the heirarchy
            if (type.BaseType != typeof(object))
            {
                foreach (var command in this.FindAllCommands(type.BaseType, instance))
                {
                    yield return command;
                }
            }
        }
    }
}