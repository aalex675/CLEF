using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CLEF.Commands;

namespace CLEF.Browsers
{
    /// <summary>
    /// Discovers Verbs, Containers, and Global Options using Attributes.
    /// </summary>
    public class AttributeObjectBrowser : IObjectBrowser
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
                    string name = property.Name;
                    IList<string> alternateNames = new List<string>();
                    string description = string.Empty;

                    var attribute = property.GetCustomAttributes(true).OfType<VerbContainerAttribute>().FirstOrDefault();
                    if (attribute != null)
                    {
                        name = attribute.Name;
                        alternateNames = attribute.AlternateNames;
                        description = attribute.Description;
                    }

                    object propertyValue = property.GetValue(instance, null);
                    yield return new ReflectionCommandContainer(name, alternateNames, description, property.PropertyType, propertyValue);
                }
            }
        }

        public IEnumerable<IOption> FindGlobalOptions(Type type, object instance)
        {
            List<IOption> globalOptions = new List<IOption>();
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
                    string name = property.Name;
                    IList<string> alternateNames = new List<string>();
                    string description = string.Empty;

                    var optionAttribute = property.GetCustomAttributes(true).OfType<OptionAttribute>().FirstOrDefault();
                    if (optionAttribute != null)
                    {
                        name = optionAttribute.Name;
                        alternateNames = optionAttribute.AlternateNames;
                        description = optionAttribute.Description;
                    }

                    var globalProperty = property;
                    object defaultValue = property.GetValue(instance, null);
                    yield return new RelayOption(name, alternateNames, description, property.PropertyType, true, defaultValue, newValue => globalProperty.SetValue(instance, newValue, null));
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

            methods = methods.Where(m => m.GetCustomAttributes(true).OfType<VerbAttribute>().Count() > 0);

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(true).OfType<VerbAttribute>().FirstOrDefault();
                if (attribute != null)
                {
                    List<IOption> options = new List<IOption>();
                    var parameters = method.GetParameters();
                    object[] optionValues = new object[parameters.Count()];
                    int i = 0;
                    foreach (var p in parameters)
                    {
                        string name = p.Name;
                        IList<string> alternateNames = new List<string>();
                        string description = string.Empty;

                        var optionAttribute = p.GetCustomAttributes(true).OfType<OptionAttribute>().FirstOrDefault();
                        if (optionAttribute != null)
                        {
                            name = optionAttribute.Name;
                            alternateNames = optionAttribute.AlternateNames;
                            description = optionAttribute.Description;
                        }

                        int index = i;
                        options.Add(new RelayOption(name, alternateNames, description, p.ParameterType, !p.IsOptional, p.DefaultValue, newValue => optionValues[index] = newValue));
                        i++;
                    }

                    yield return new ReflectionCommand(attribute.Name, attribute.Description, options, optionValues, instance, method, attribute.AlternateNames);
                }
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