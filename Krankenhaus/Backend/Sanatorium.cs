using Krankenhaus.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Sanatorium
    {
        private List<Patient> patients;
        private string fileName;
        private Random rand = new Random();
        private Logger logger;
        private int improvementPercentage;
        private int deteriorationPercentage;
        private AfterLife afterlife;
        private Survivors survivors;
        private ReadFromFile readFromFile;

        public int NumberOfBeds { get; private set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }
        public bool Saving { get; private set; }

        public Sanatorium()
        {
            patients = new List<Patient>();
            logger = new Logger();
            readFromFile = new ReadFromFile();
            afterlife = AfterLife.GetInstance();
            survivors = Survivors.GetInstance();
            fileName = "Sanatorium.txt";
            Saving = false;

            // Made to be able to alter value in the future
            NumberOfBeds = 10;
            improvementPercentage = 35;
            deteriorationPercentage = 50;
        }
        public void CheckIn(Patient patient)
        {
            patient.ArrivalToHospital = DateTime.Now;
            patients.Add(patient);
        }

        public async void OnTick(object sender, EventArgs e)
        {
            if (patients.Count != 0)
            {
                await SaveToFile();
                await UpdatePatients();
            }
        }

        private async Task UpdatePatients()
        {
            var remove = new List<Patient>();

            // Calculates the chance of improving or declining the sickness level for each patient

            foreach (Patient patient in patients)
            {
                #region Sickness level calculation
                int chance = rand.Next(1, 101);

                if (chance <= improvementPercentage)
                {
                    patient.SicknessLevel -= 1;
                }
                else if (chance <= improvementPercentage + deteriorationPercentage)
                {
                    patient.SicknessLevel += 1;
                }

                // If none of these conditions are met, the patient will keep its current sickness level

                #endregion

                // Adds patient to either survivors list or afterlife list 
                #region Dead or Survivor
                if (patient.SicknessLevel <= 0)
                {
                    while (survivors.Saving)
                    {
                        await Task.Delay(1);
                    }
                    survivors.Add(patient);
                    patient.DepartureFromHospital = DateTime.Now;
                    remove.Add(patient);
                }
                else if (patient.SicknessLevel >= 10)
                {
                    while (afterlife.Saving)
                    {
                        await Task.Delay(1);
                    }
                    afterlife.Add(patient);
                    patient.DepartureFromHospital = DateTime.Now;
                    remove.Add(patient);
                }
                #endregion
            }

            // Removes the patients that were either dead or cured, from the patient list
            foreach (Patient patient in remove)
            {
                if (patients.Contains(patient))
                {
                    patients.Remove(patient);
                }
            }

            // If there are no patients left this method will not be called again, so we have to update the file to be empty here
            if (patients.Count == 0)
            {
                await SaveToFile();
            }
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

        /// <summary>
        /// Saves all the patients and the current doctor to a file
        /// </summary>
        private async Task SaveToFile()
        {
            Saving = true;
            await Task.Delay(1);
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
    }
}
