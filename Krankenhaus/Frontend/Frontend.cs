using Krankenhaus.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Frontend
    {
        private AfterLife afterlife;
        private Survivors survivors;
        private ReadFromFile read;
        public Frontend()
        {
            Generator.UpdateStatus += PrintStatusReport;
            afterlife = AfterLife.GetInstance();
            survivors = Survivors.GetInstance();
            read = new ReadFromFile();
        }

        public bool ReadData(out int ticksReturn)
        {
            ticksReturn = 0;
            if (read.SessionExists(out int ticks))
            {
                while (true)
                {
                    Console.WriteLine("There is an existing file. Do you want to use it? Y / N ");
                    string answer = Console.ReadLine().ToUpper();

                    if (answer == "Y")
                    {
                        ticksReturn = ticks;
                        return true;
                    }
                    else if (answer == "N")
                    {
                        return false;
                    }
                }
            }
            return false;
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
        public int DoctorInput()
        {
            do
            {
                Console.WriteLine("How many doctors should there be? [1-10]");
                bool check = int.TryParse(Console.ReadLine(), out int result);
                if (result <= 10 && result >= 1)
                {
                    return result;
                }
            } while (true);

        }
        public int PatientInput()
        {
            do
            {
                Console.WriteLine("How many patients do you want? [5-30]");
                bool check = int.TryParse(Console.ReadLine(), out int result);
                if (result <= 30 && result >= 5)
                {
                    return result;
                }
            } while (true);
        }

        public void PrintStatusReport(object sender, UpdateStatusArgs e)
        {
            Console.WriteLine(e.Status);
        }

        public void DisplayResult(object sender, TimeTickArgs t)
        {
            Console.Clear();

            Console.WriteLine("Amount of ticks: {0}", t.Ticks);
            Console.WriteLine("Start time was: {0}\n\n", t.StartTime);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The people who died, moment of silence");
            Console.WriteLine("Amount of people: {0}\n", afterlife.Length);
            Console.ResetColor();


            var dead = afterlife.GetPatients();

            foreach (var item in dead)
            {
                TimeSpan ts = item.DepartureFromHospital - item.ArrivalToHospital;

                if (ts.Seconds < 1)
                {
                    Console.WriteLine("Name: {0,-20} | Time spent in hospital: None, died in queue ", item.PatientName);
                }
                else
                {
                    Console.WriteLine("Name: {0,-20} | Time spent in hospital: {1} days ", item.PatientName, ts.Seconds);
                }
                
            }


            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The people who survived, hooray");
            Console.WriteLine("Amount of people: {0}\n", survivors.Length);
            Console.ResetColor();

            var survived = survivors.GetPatients();
            foreach (var item in survived)
            {
                TimeSpan ts = item.DepartureFromHospital - item.ArrivalToHospital;
                Console.WriteLine("Name: {0,-20} | Time spent in hospital: {1} days", item.PatientName, ts.Seconds);
            }
        }
    }
}
