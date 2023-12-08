using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public class Engine
    {
        protected static Assembly _assembly;
        protected static string _assemblyName;

        protected static ControllerCollection _controllerMap;
        public static T GetController<T>(string name)
            where T : Controller
        {
            return (T)_controllerMap.CreateController(name);
        }

        public static void Register(object app)
        {
            Register(app, null);
        }

        public static void Register(object app, string assemblyName)
        {
            _assembly = app.GetType().Assembly;
            _assemblyName = assemblyName ?? _assembly.GetName().Name;

            _controllerMap = new ControllerCollection(_assembly);
        }

        public static T CreateObject<T>(string name)
        {
            return (T)_assembly.CreateInstance(_assemblyName + '.' + name);
        }

        static Stack<Thread> _threads;
        public static Thread BeginInvoke(Action action)
        {
            if (_threads == null)
            {
                _threads = new Stack<Thread>();
            }
            while (_threads.Count > 0)
            {
                if (_threads.Peek().IsAlive) break;
                _threads.Pop();
            }
            var th = new Thread(new ThreadStart(action));
            _threads.Push(th);

            th.Start();
            return th;
        }

        public static void Exit()
        {
            if (_threads != null)
            {
                while (_threads.Count > 0)
                {
                    var th = _threads.Pop();
                    if (th.IsAlive)
                        th.Abort();
                }
            }
        }
    }
}
