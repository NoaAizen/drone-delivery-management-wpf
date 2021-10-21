using System;
using System.Collections.Generic;

namespace Targil0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome3394();
            Welcome8965();
            Console.ReadKey();
        }

        private static void Welcome3394()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", name);
        }

        static partial void Welcome8965();

    }
}

