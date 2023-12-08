using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public class Screen
    {
        static public void Write(string text)
        {
            Console.Write(text);
        }
        static public void Write(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        static public void Message(ConsoleColor color, string text)
        {
            Write(color, text);
            Console.WriteLine();
        }
        static public void Message(string text)
        {
            Message(ConsoleColor.Gray, text);
        }
        static public void Message(ConsoleColor color, string format, params object[] values)
        {
            Message(color, string.Format(format, values));
        }
        static public void Now(string what)
        {
            Message(ConsoleColor.Magenta, "[{0:dd.MM.yyyy HH:mm:ss}] {1}", DateTime.Now, what);
        }
        static public void Now() { Now(null); }
        static public void Info(string text)
        {
            Message(ConsoleColor.Cyan, text);
        }
        static public void Error(string text)
        {
            Message(ConsoleColor.Red, text);
        }
        static public void Warning(string text)
        {
            Message(ConsoleColor.Yellow, text);
        }
        static public void Success(string text)
        {
            Message(ConsoleColor.Green, text);
        }
        static public void Waiting(string text)
        {
            Message(ConsoleColor.White, text + " ...");
        }
    }
}
