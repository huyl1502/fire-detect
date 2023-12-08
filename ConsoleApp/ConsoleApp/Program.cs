using ConsoleApp.Common;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Engine.Register(new Program());

            while (true) { }
        }
    }
}
