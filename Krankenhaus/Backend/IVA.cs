using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
        public int NumberOfBeds { get; private set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }
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
            NumberOfBeds = 5;
            fileName = "IVA.txt";
            doctorsFile = "Doctors.txt";
            improvementPercentage = 70;
            deteriorationPercentage = 10;
        }

        public void MakeDoctors(int userInput)
        {
            for (int i = 0; i < userInput; i++)
            {
                Doctor doctor = new Doctor();
                doctors.Enqueue(doctor);
                Thread.Sleep(250);
            }
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
                await UpdatePatients();
                if (IsDoctorPresent)
                {
                    await AdjustDoctors();
                }
                await SaveToFile();
            }
        }

        private async Task UpdatePatients()
        {
            var remove = new List<Patient>();

            if ((IsDoctorPresent == false) && (doctors.Count != 0))
            {
                doctor = NextDoctor();
                IsDoctorPresent = true;
                await SaveDoctorsToFile();
            }

            foreach (Patient patient in patients)
            {
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


                if (patient.SicknessLevel <= 0)
                {
                    Generator.survivors.Add(patient);
                    patient.DepartureFromHospital = DateTime.Now;
                    remove.Add(patient);
                }
                else if (patient.SicknessLevel >= 10)
                {
                    Generator.afterlife.Add(patient);
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
        }

        private async Task AdjustDoctors()
        {

            doctor.Fatigue += 5;

            if (doctor.Fatigue == 20)
            {
                if (doctors.Count != 0)
                {
                    doctor = NextDoctor();
                    await SaveDoctorsToFile();
                }
                else
                {
                    IsDoctorPresent = false;
                }
            }

        }

        private async Task SaveToFile()
        {
            if (patients.Count == 0)
            {
                await logger.LogToFile(fileName, " ", false);
            }
            else
            {
                bool appendLine = false;
                if (IsDoctorPresent)
                {
                    await logger.LogToFile(fileName, "Extra doctor: " + doctor.ToString(), appendLine);
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
                        await logger.LogToFile(fileName, patient.ToString(), appendLine);
                        appendLine = true;
                    }
                }
            }
        }
        private async Task SaveDoctorsToFile()
        {
            bool appendLine = false;

            if (doctors.Count == 0)
            {
                await logger.LogToFile(doctorsFile, " ", appendLine);
            }
            else
            {
                foreach (Doctor doctor in doctors)
                {
                    await logger.LogToFile(doctorsFile, doctor.ToString(), appendLine);
                    appendLine = true;
                }
            }
        }
        private Doctor NextDoctor()
        {
            return doctors.Dequeue();
        }
    }
}
