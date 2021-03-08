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
        public int OccupiedBeds { get; set; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }

        public IVA()
        {
            patients = new List<Patient>();
        }

        public bool CheckIn(Patient patient)
        {
            if(IsFull)
            {
                return false;
            }

            patients.Add(patient);
            return true;
        }

        public void OnTick(object sender, EventArgs e)
        {
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
            }
        }

        public void CheckOut()
        {
            
        }
    }
}
