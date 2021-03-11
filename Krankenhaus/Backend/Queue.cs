using Krankenhaus.Backend;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Queue
    {
        private Queue<Patient> patients;
        private Random rand = new Random();
        private string fileName;
        private Logger logger;
        private int improvementPercentage;
        private int deteriorationPercentage;
        private ReadFromFile readFromFile;

        public Queue()
        {
            patients = new Queue<Patient>();
            logger = new Logger();
            fileName = "Queue.txt";
            improvementPercentage = 5;
            deteriorationPercentage = 80;
            readFromFile = new ReadFromFile();
            Saving = false;
        }

        public bool Saving { get; private set; }
        public int Length { get => patients.Count; }

        public Patient GetNextPatient()
        {
            return patients.Dequeue();
        }

        public Patient Peek()
        {
            return patients.Peek();
        }
        
        public void AddToQueue(Patient patient)
        {
            patients.Enqueue(patient);
        }

        public async void OnTick(object sender, EventArgs e)
        {
            await SaveToFile();

            if (patients.Count != 0)
            {
                UpdatePatients();
            }
        }


        private void UpdatePatients()
        {
            // Calculates the chance of improving or declining the sickness level for each patient

            foreach (Patient patient in patients)
            {
                int chance = rand.Next(1, 101);

                if (patient.SicknessLevel == 0 || patient.SicknessLevel == 10)
                {
                    patient.SicknessLevel = patient.SicknessLevel;
                }
                else if (chance <= improvementPercentage)
                {
                    patient.SicknessLevel -= 1;
                }
                else if (chance <= (improvementPercentage + deteriorationPercentage))
                {
                    patient.SicknessLevel += 1;
                }

                // If none of these conditions are met, the patient will keep its current sickness level
            }
        }

        /// <summary>
        /// Reads data from text file and populates the patient queue
        /// </summary>
        internal void ReadData(object sender, EventArgs e)
        {
            var data = readFromFile.GetPeopleList(fileName);

            foreach (var person in data)
            {
                patients.Enqueue((Patient)person);
            }
        }

        /// <summary>
        /// Saves all the patients to a file
        /// </summary>
        private async Task SaveToFile()
        {
            Saving = true;
            await Task.Delay(1);
            bool appendLine = false;

            if (patients.Count == 0)
            {
                await logger.LogToFile(fileName, " ", false);
            }
            else
            {
                if (patients.Count == 0)
                {
                    await logger.LogToFile(fileName, " ", appendLine);
                }
                else
                {
                    foreach (Patient patient in patients)
                    {
                        await logger.LogToFile(fileName, patient.ToFileFormat(), appendLine);
                        appendLine = true;
                    }
                }
            }
            Saving = false;
        }

        /// <summary>
        /// Returns a list of all patients in the queue
        /// </summary>
        public List<Patient> GetAllPatients()
        {
            List<Patient> toReturn = new List<Patient>();

            foreach (var pat in patients)
            {
                toReturn.Add(pat);
            }
            return toReturn;
        }
    }
}
