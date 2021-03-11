using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Krankenhaus.Backend;

namespace Krankenhaus
{
    class IVA
    {
        private bool doctorPresent = false;
        private List<Patient> patients;
        private Queue<Doctor> doctors;
        private Doctor doctor;
        private string fileName;
        private string doctorsFile;
        private Logger logger;
        private Random rand = new Random();
        private int improvementPercentage;
        private int deteriorationPercentage;
        private AfterLife afterlife;
        private Survivors survivors;
        private ReadFromFile readFromFile;
        public int NumberOfBeds { get; private set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }
        public bool Saving { get; private set; }
        public bool IsDoctorPresent
        {
            get { return doctorPresent; }
            private set { doctorPresent = value; }
        }

        public IVA()
        {
            logger = new Logger();
            patients = new List<Patient>();
            doctors = new Queue<Doctor>();
            afterlife = AfterLife.GetInstance();
            survivors = Survivors.GetInstance();
            fileName = "IVA.txt";
            doctorsFile = "Doctors.txt";
            readFromFile = new ReadFromFile();
            Saving = false;

            // Made to be able to alter value in the future
            NumberOfBeds = 5;
            improvementPercentage = 70;
            deteriorationPercentage = 10;
        }

        /// <summary>
        /// Generates doctors with random names and sickness level
        /// </summary>
        /// <param name="userInput">Number of patients to be generated</param>
        public void MakeDoctors(int userInput)
        {
            for (int i = 0; i < userInput; i++)
            {
                Doctor doc = new Doctor();
                doctors.Enqueue(doc);
                Thread.Sleep(250);
            }
        }

        public void CheckIn(Patient patient)
        {
            patient.ArrivalToHospital = DateTime.Now;
            patients.Add(patient);
        }

        /// <summary>
        /// Reads data from text file and populates the patient list
        /// </summary>
        internal void ReadData(object sender, EventArgs e)
        {
            var data = readFromFile.GetPeopleList(fileName);

            foreach (var person in data)
            {
                if (person.Title == Title.Doctor)
                {
                    doctor = (Doctor)person;
                    
                    IsDoctorPresent = true;
                }
                else
                {
                    patients.Add((Patient)person);
                }
            }
        }

        /// <summary>
        /// Reads data from text file and populates the doctor list
        /// </summary>
        internal void ReadDoctors(object sender, EventArgs e)
        {
            var data = readFromFile.GetPeopleList(doctorsFile);

            foreach (var person in data)
            {
                Doctor doc = (Doctor)person;
                doctors.Enqueue(doc);
            }
        }

        public async void OnTick(object sender, EventArgs e)
        {
            if (patients.Count != 0)
            {
                await SaveToFile();
                await SaveDoctorsToFile();
                await UpdatePatients();
                if (IsDoctorPresent)
                {
                    AdjustDoctors();
                }
            }
            else
            {
                IsDoctorPresent = false;
            }
        }

        private async Task UpdatePatients()
        {
            var remove = new List<Patient>();

            if ((IsDoctorPresent == false) && (doctors.Count != 0)) // If there is no doctor and if there are doctors in the queue, get new doctor
            {
                doctor = NextDoctor();
                IsDoctorPresent = true;
            }

            // Calculates the chance of improving or declining the sickness level for each patient
            
            foreach (Patient patient in patients)
            {
                #region Sickness level calculation
                int chance = rand.Next(1, 101);
                int competence = 0;
                int percentageAdjustment = 0;

                if (IsDoctorPresent == true)
                {
                    competence = doctor.Competence;
                    percentageAdjustment = ((competence / 2));
                }

                if (chance <= (improvementPercentage + competence))
                {
                    patient.SicknessLevel -= 1;
                }
                else if (chance >= (improvementPercentage + deteriorationPercentage - percentageAdjustment))
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
        /// Adjusts the current doctor's fatigue, 
        /// and brings in a new doctor when the current is exhausted
        /// </summary>
        private void AdjustDoctors()
        {
            doctor.Fatigue += 5;

            if (doctor.Fatigue == 20)
            {
                if (doctors.Count != 0)
                {
                    doctor = NextDoctor();
                }
                else
                {
                    IsDoctorPresent = false;
                }
            }
        }

        public async void ClearDoctorsFile(object sender, TimeTickArgs e)
        {
            await logger.LogToFile(doctorsFile, " ", false);
        }

        /// <summary>
        /// Saves all the patients and the current doctor to a file
        /// </summary>
        private async Task SaveToFile()
        {
            Saving = true;
            if (patients.Count == 0)
            {
                await logger.LogToFile(fileName, " ", false);
            }
            else
            {
                bool appendLine = false;
                if (IsDoctorPresent)
                {
                    await logger.LogToFile(fileName, doctor.ToString(), appendLine);
                    appendLine = true;
                }

                if (patients.Count == 0)
                {
                    await logger.LogToFile(fileName, " ", false);
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
        /// Saves all the doctors in the queue
        /// </summary>
        private async Task SaveDoctorsToFile()
        {
            Saving = true;
            await Task.Delay(1);
            bool appendLine = false;

            if (doctors.Count == 0)
            {
                await logger.LogToFile(doctorsFile, " ", appendLine);
            }
            else
            {
                foreach (Doctor doc in doctors)
                {
                    await logger.LogToFile(doctorsFile, doc.ToString(), appendLine);
                    appendLine = true;
                }
            }
            Saving = false;
        }

        private Doctor NextDoctor()
        {
            return doctors.Dequeue();
        }
    }
}
