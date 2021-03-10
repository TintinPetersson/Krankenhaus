using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus.Backend
{
    class AfterLife
    {
        private List<Patient> patients;
        private Logger logger;
        private string fileName;

        public int Length { get => patients.Count; }

        public AfterLife()
        {
            logger = new Logger();
            patients = new List<Patient>();
            fileName = "Afterlife.txt";
        }

        public void Add(Patient patient)
        {
            patients.Add(patient);
            SaveToFile(patient);
        }

        private async void SaveToFile(Patient patient)
        {
            await logger.AddToBuffer(fileName, patient.ToString()); 
        }
    }
}
