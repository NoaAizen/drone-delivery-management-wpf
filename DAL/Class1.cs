using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{

    partial class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>();
            List<int> numbers1 = new List<int>(5);
            Console.Write(numbers1.Capacity);
            Console.ReadKey();
        }

   
    }

}
