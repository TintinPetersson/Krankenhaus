using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Program
    {
        static void Main(string[] args)
        {
            //bara iva clearar
            Generator generator = new Generator();
            generator.Start();

            Console.ReadLine();
        }
    }
}
