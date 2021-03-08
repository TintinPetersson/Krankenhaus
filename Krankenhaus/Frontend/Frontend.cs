using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Frontend
    {
        public Frontend()
        {
            Generator.UpdateStatus += PrintStatusReport;
        }

        public int Menu()
        {
            do
            {
                Console.WriteLine("Choose the speed of the program [1-10 seconds]: ");
                bool check = int.TryParse(Console.ReadLine(), out int result);
                if (result <= 10 && result >= 1)
                {
                    return result;
                }
            } while (true);
        }

        public void PrintStatusReport(object sender, UpdateStatusArgs e)
        {
            Console.WriteLine(e.Status);
        }
    }
}
