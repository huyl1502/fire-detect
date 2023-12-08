    using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class ControllerCollection : Dictionary<string, Type>
    {
        public ControllerCollection(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                var name = type.Name.ToLower();
                if (name.EndsWith("controller"))
                {
                    this.Add(name.Substring(0, name.Length - 10), type);
                }
            }
        }
        public Controller CreateController(string name)
        {
            Type type;
            if (this.TryGetValue(name.ToLower(), out type))
            {
                return (Controller)Activator.CreateInstance(type);
            }
            return null;
        }
    }

    public partial class Controller
    {
        public string ControllerName
        {
            get
            {
                var name = this.GetType().Name;
                return name.Substring(0, name.Length - 10);
            }
        }

        public MethodInfo GetMethod(string name)
        {
            name = name.ToLower();
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.Name.ToLower() == name)
                {
                    return method;
                }
            }
            return null;
        }
        public MethodInfo GetMethod(string name, object[] values)
        {
            MethodInfo result = null;

            name = name.ToLower();
            foreach (var method in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if (method.Name.ToLower() != name)
                {
                    continue;
                }

                var parameters = method.GetParameters();
                if (parameters.Length != values.Length) continue;

                result = method;

                for (int i = 0; i < values.Length; i++)
                {
                    var type = parameters[i].ParameterType;
                    object v = values[i];
                    if (v == null)
                    {
                        if (type.IsValueType)
                        {
                            result = null;
                            break;
                        }
                        continue;
                    }

                    if (v.GetType() != type)
                    {
                        try
                        {
                            values[i] = Convert.ChangeType(v, type);
                        }
                        catch
                        {
                            result = null;
                            break;
                        }
                    }
                }

                if (result != null) { break; }
            }
            return result;
        }

        protected virtual void OnExecuteError(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.WriteLine(e);
            Console.ResetColor();
        }
    }
}
