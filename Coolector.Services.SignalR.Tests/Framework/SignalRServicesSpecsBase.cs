using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Coolector.Services.SignalR.Tests.Framework
{
    public abstract class SignalRServicesSpecsBase
    {
        protected static T Cast<T>(T example, object o)
        {
            IComparer<string> comparer = StringComparer.CurrentCultureIgnoreCase;
            //Get constructor with lowest number of parameters and its parameters 
            var constructor = typeof(T).GetConstructors(
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            ).OrderBy(c => c.GetParameters().Length).First();
            var parameters = constructor.GetParameters();

            //Get properties of input object
            var sourceProperties = new List<PropertyInfo>(o.GetType().GetProperties());
            if (parameters.Length <= 0)
                return (T)constructor.Invoke(null);

            var values = new object[parameters.Length];
            for (int i = 0; i < values.Length; i++)
            {
                Type t = parameters[i].ParameterType;
                //See if the current parameter is found as a property in the input object
                var source = sourceProperties.Find(item => comparer.Compare(item.Name, parameters[i].Name) == 0);

                //See if the property is found, is readable, and is not indexed
                if (source != null && source.CanRead &&
                    source.GetIndexParameters().Length == 0)
                {
                    //See if the types match.
                    if (source.PropertyType == t)
                    {
                        //Get the value from the property in the input object and save it for use
                        //in the constructor call.
                        values[i] = source.GetValue(o, null);
                        continue;
                    }
                    //See if the property value from the input object can be converted to
                    //the parameter type
                    try
                    {
                        values[i] = Convert.ChangeType(source.GetValue(o, null), t);
                        continue;
                    }
                    catch
                    {
                        //Impossible. Forget it then.
                    }
                }
                //If something went wrong (i.e. property not found, or property isn't 
                //converted/copied), get a default value.
                values[i] = t.GetTypeInfo().IsValueType ? Activator.CreateInstance(t) : null;
            }
            //Call the constructor with the collected values and return it.
            return (T)constructor.Invoke(values);
            //Call the constructor without parameters and return the it.
        }
    }
}