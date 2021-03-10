using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus.Backend
{
    class Survivors
    {
        private List<Patient> patients;
        private Logger logger;
        private string fileName;
        private static Survivors survivors;

        public int Length { get => patients.Count; }

        private Survivors()
        {
            logger = new Logger();
            patients = new List<Patient>();
            fileName = "Survivors.txt";
        }

        public static Survivors GetInstance()
        {
            if (survivors == null)
            {
                survivors = new Survivors();
            }

            return survivors;
        }

        public void Add(Patient patient)
        {
            patients.Add(patient);
        }

        public List<Patient> GetPatients()
        {
            var toReturn = new List<Patient>();
            foreach (Patient patient in patients)
            {
                toReturn.Add(patient);
            }
            return toReturn;
        }

        //public async void OnTick(object sender, EventArgs e)
        //{
        //    await SaveToFile();
        //}

        public async Task SaveToFile()
        {
            if (patients.Count == 0)
            {
                await logger.LogToFile(fileName, " ", false);
            }
            else
            {
                bool appendLine = false;
                foreach (Patient patient in patients)
                {
                    await logger.LogToFile(fileName, patient.ToString(), appendLine);
                    appendLine = true;
                }
            }
        }
    }
}
