using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class IVA
    {
        private bool doctorPresent = false;
        private List<Patient> patients;
        private Doctor doctor;
        Random rand = new Random();
        public int NumberOfBeds { get; set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }


        public bool DoctorPresent
        {
            get { return doctorPresent; }
            set { doctorPresent = value; }
        }

        public IVA()
        {
            patients = new List<Patient>();
            NumberOfBeds = 5;
        }

        public void CheckIn(Patient patient)
        {
            patients.Add(patient);
        }

        public void OnTick(object sender, EventArgs e)
        {
            var remove = new List<Patient>();


            if (DoctorPresent == false && Generator.doctorsQueue.Count != 0)
            {
                doctor = NextDoctor();
                DoctorPresent = true;
            }


            foreach (Patient patient in patients)
            {
                int newSickness = rand.Next(1, 21);

                if (DoctorPresent == true)
                {
                    newSickness += doctor.Competence;
                }

                if (newSickness <= 14)
                {
                    newSickness = patient.SicknessLevel - 1;
                }
                else if (newSickness <= 16)
                {
                    newSickness = patient.SicknessLevel + 1;
                }
                else
                {
                    newSickness = patient.SicknessLevel;
                }

                patient.SicknessLevel = newSickness;

                if (patient.SicknessLevel <= 0)
                {
                    Generator.survivors.Add(patient);
                    remove.Add(patient);
                }
                else if (patient.SicknessLevel >= 10)
                {
                    Generator.afterlife.Add(patient);
                    remove.Add(patient);

                }

            }

            doctor.Fatigue += 5;

            if (doctor.Fatigue == 20 && Generator.doctorsQueue.Count != 0)
            {
               doctor = NextDoctor();

            }
            if (Generator.doctorsQueue.Count == 0)
            {
                DoctorPresent = false;
            }

            foreach (Patient patient in remove)
            {


                if (patients.Contains(patient))
                {
                    patients.Remove(patient);
                }
            }
        }

        public Doctor NextDoctor()
        {
            Doctor nextDoc = Generator.doctorsQueue.Dequeue();

            return nextDoc;
        }
    }
}
