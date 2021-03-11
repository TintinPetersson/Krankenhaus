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
            NumberOfBeds = 10;
            logger = new Logger();
            afterlife = AfterLife.GetInstance();
            survivors = Survivors.GetInstance();
            fileName = "Sanatorium.txt";
            improvementPercentage = 35;
            deteriorationPercentage = 50;
            readFromFile = new ReadFromFile();
            Saving = false;
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
                //await afterlife.SaveToFile();
                //await survivors.SaveToFile();
                await UpdatePatients();
            }
        }

        private async Task UpdatePatients()
        {
            var remove = new List<Patient>();

            foreach (Patient patient in patients)
            {
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
            }

            foreach (Patient patient in remove)
            {
                if (patients.Contains(patient))
                {
                    patients.Remove(patient);
                }
            }

            if (patients.Count == 0)
            {
                await SaveToFile();
            }
        }

        internal void ReadData(object sender, EventArgs e)
        {
            var data = readFromFile.GetPeopleList(fileName);

            foreach (var person in data)
            {
                patients.Add((Patient)person);
            }

        }

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
                    await logger.LogToFile(fileName, patient.ToString(), appendLine);
                    appendLine = true;
                }
            }
            Saving = false;
        }
    }
}
