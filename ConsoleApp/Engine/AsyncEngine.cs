using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System
{
    public class AsyncEngine : Engine
    {
        static Queue<Thread> _watings;
        static Stack<Thread> _running;

        public static void CreateThread(Action exec)
        {
            while (_running.Count > 0 && _running.Peek().IsAlive == false)
            {
                _running.Pop();
            }

            var ts = new ThreadStart(() => exec.Invoke());
            _watings.Enqueue(new Thread(ts));

            LoadThread();
        }

        static void LoadThread()
        {
            while (_watings.Count > 0)
            {
                Thread.Sleep(100);

                var t = _watings.Dequeue();
                _running.Push(t);

                t.Start();
            }
        }

        public static void End()
        {
            while (_running.Count > 0)
            {
                _running.Pop().Abort();
            }
        }
    }
}
