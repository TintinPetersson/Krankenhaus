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
            Ticker ticker = new Ticker();
            Doctor doctor = new Doctor();
            Patient patient = new Patient();

            ticker.Tick += doctor.VetEjHandler;
            ticker.Tick += patient.VetEjHandler;
        }
    }
}
