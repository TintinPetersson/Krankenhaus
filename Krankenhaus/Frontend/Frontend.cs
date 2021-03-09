﻿using Krankenhaus.Backend;
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

        public void DisplayResult(object sender, TimeTickArgs t)
        {
            Console.Clear();

            Console.WriteLine("Amount of ticks: {0}", t.Ticks);
            Console.WriteLine("Start time was: {0}\n\n", t.StartTime);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("The people who died, moment of silence");
            Console.WriteLine("Amount of people: {0}\n", Generator.afterlife.Count);
            Console.ResetColor();
            foreach (var item in Generator.afterlife)
            {
                Console.WriteLine("Name: {0}", item.PatientName);
            }
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("The people who survived, hooray");
            Console.WriteLine("Amount of people: {0}\n", Generator.survivors.Count);
            Console.ResetColor();
            foreach (var item in Generator.survivors)
            {
                Console.WriteLine("Name: {0}", item.PatientName);
            }
        }
    }
}
