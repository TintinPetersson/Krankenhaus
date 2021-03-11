using Krankenhaus.Backend;
using System;
using System.Collections.Generic;

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

        /// <summary>
        /// If there are any existing files, asks the user if it wants to read from them
        /// </summary>
        /// <param name="ticksReturn">Which tick the simulation was on in the last session</param>
        /// <returns>If data should be read from files</returns>
        public bool ReadData(out int ticksReturn)
        {
            ticksReturn = 0;
            if (read.SessionExists(out int ticks))
            {
                while (true)
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("There is an existing file. Do you want to use it? ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Y ");
                    Console.ResetColor();
                    Console.Write("/ ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("N");
                    Console.ResetColor();
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

        /// <summary>
        /// Lets the user choose the length of the ticks
        /// </summary>
        /// <returns>The length of ticks in seconds</returns>
        public int GetSpeed()
        {
            do
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Choose the speed of the program [");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("1 - 10 seconds");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("]: ");
                Console.ResetColor();
                bool check = int.TryParse(Console.ReadLine(), out int result);
                if (result <= 10 && result >= 1)
                {
                    return result;
                }
            } while (true);
        }

        /// <summary>
        /// Lets the user choose how many doctors should be generated
        /// </summary>
        public int DoctorInput()
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("How many doctors should there be [");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("1 - 10");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("]:");
                Console.ResetColor();
                bool check = int.TryParse(Console.ReadLine(), out int result);
                if (result <= 10 && result >= 1)
                {
                    return result;
                }
            } while (true);

        }

        /// <summary>
        /// Lets the user choose how many patients should be generated
        /// </summary>
        public int PatientInput()
        {
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("How many patients do you want [");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("5 - 30");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("]:");
                Console.ResetColor();
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

        /// <summary>
        /// Displays the end result
        /// </summary>
        public void DisplayResult(object sender, TimeTickArgs t)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Amount of ticks: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("{0}", t.Ticks);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Start time was: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("{0}\n\n", t.StartTime);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("The people who ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("died");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(", moment of silence");
            Console.Write("Amount: ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("{0}\n", afterlife.Length);
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
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("The people who ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("survived");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(", hooray");
            Console.Write("Amount: ");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("{0}\n", survivors.Length);
            Console.ResetColor();

            var survived = survivors.GetPatients();
            foreach (var item in survived)
            {
                TimeSpan ts = item.DepartureFromHospital - item.ArrivalToHospital;
                Console.WriteLine("Name: {0,-20} | Time spent in hospital: {1} days", item.PatientName, ts.Seconds);
            }
        }

        /// <summary>
        /// Displays a list of optional data type
        /// </summary>
        public void DisplayList<T>(List<T> list)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Displaying all patients...\n");
            Console.ResetColor();
            foreach (T type in list)
            {
                Console.Write(type.ToString());
            }
        }

    }
}
