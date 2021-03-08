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
        Random rand = new Random();

        public int NumberOfBeds { get; set; }
        public int OccupiedBeds { get => patients.Count; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }

        public Sanatorium()
        {
            patients = new List<Patient>();
            NumberOfBeds = 10;
        }
        public void CheckIn(Patient patient)
        {
            patients.Add(patient);
        }
        public void CheckOut()
        {

        }

        public void OnTick(object sender, EventArgs e)
        {
            var remove = new List<Patient>();

            foreach (Patient patient in patients)
            {
                int newSickness = rand.Next(1, 21);

                if (newSickness <= 7)
                {
                    newSickness = patient.SicknessLevel - 1;
                }
                else if (newSickness <= 17)
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

            foreach (Patient patient in remove)
            {
                if (patients.Contains(patient))
                {
                    patients.Remove(patient);
                }
            }


        }
    }
}
