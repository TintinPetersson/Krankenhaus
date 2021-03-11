using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus.Backend
{
    /// <summary>
    /// Created as a singleton, in order to instantiate the same instance of the object
    /// </summary>
    class AfterLife
    {
        private List<Patient> patients;
        private Logger logger;
        private string fileName;
        private static AfterLife afterLife;
        private ReadFromFile readFromFile;

        public bool Saving { get; private set; }
        public int Length { get => patients.Count; }

        private AfterLife()
        {
            Saving = false;
            logger = new Logger();
            patients = new List<Patient>();
            fileName = "Afterlife.txt";
            readFromFile = new ReadFromFile();
        }

        public static AfterLife GetInstance()
        {
            if (afterLife == null)
            {
                afterLife = new AfterLife();
            }
            return afterLife;
        }

        public void Add(Patient patient)
        {
            patients.Add(patient);
        }

        /// <summary>
        /// Returns a list of all the patients in afterlife
        /// </summary>
        public List<Patient> GetPatients()
        {
            var toReturn = new List<Patient>();
            foreach (Patient patient in patients)
            {
                toReturn.Add(patient);
            }
            return toReturn;
        }

        public async void OnTick(object sender, EventArgs e)
        {
            await SaveToFile();
        }

        public async void ClearFile(object sender, TimeTickArgs e)
        {
            await logger.LogToFile(fileName, " ", false);
        }

        public async Task SaveToFile()
        {
            Saving = true;
            await Task .Delay(1);
            if (patients.Count == 0)
            {
                await logger.LogToFile(fileName, " ", false);
            }
            else
            {
                bool appendLine = false;
                foreach (Patient patient in patients)
                {
                    await logger.LogToFile(fileName, patient.ToFileFormat(), appendLine);
                    appendLine = true;
                }
            }
            Saving = false;
        }

        /// <summary>
        /// Reads data from text file and populates the patient list
        /// </summary>
        internal void ReadData(object sender, EventArgs e)
        {
            var data = readFromFile.GetPeopleList(fileName);

            foreach (var person in data)
            {
                patients.Add((Patient)person);
            }
        }
    }
}
