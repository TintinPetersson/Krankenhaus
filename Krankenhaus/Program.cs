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
            Generator.Start();
            Console.ReadLine();
            //ticker.Tick += doctor.VetEjHandler;
            //ticker.Tick += patient.VetEjHandler;
        }

    }
}
