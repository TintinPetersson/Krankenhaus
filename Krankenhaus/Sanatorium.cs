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

        public int NumberOfBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public bool IsFull { get => NumberOfBeds - OccupiedBeds <= 0; }

        public Sanatorium()
        {
            patients = new List<Patient>();
        }

        public bool CheckIn(Patient patient)
        {
            if (IsFull)
            {
                return false;
            }

            patients.Add(patient);
            return true;
        }

        public void CheckOut()
        {

        }

    }
}
