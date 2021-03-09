using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class IVA
    {
        
        private List<Patient> patients;
        Random rand = new Random();
        public int NumberOfBeds { get; set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }


        bool DoctorPresent { get; set; }

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

            
            if (DoctorPresent == false)
            {
                DoctorArrive();
                DoctorPresent = true;
            }
            

            foreach (Patient patient in patients)
            {
                int newSickness = rand.Next(1, 21);


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

            foreach(Patient patient in remove)
            {
                

                if (patients.Contains(patient))
                {
                    patients.Remove(patient);
                }
            }
        }

        public Doctor DoctorArrive()
        {
            Doctor nextDoc = Generator.doctorsQueue.Dequeue();
            //competence = nextDoc.Competence;

            return nextDoc;
        }
    }
}
