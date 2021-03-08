using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    public class Doctor
    {
        Random rand = new Random();

        private int doctorId;
        private string doctorName;
        private int competence;
        private int fatigue;

        public int DoctorID
        {
            get { return doctorId; }
            set { doctorId = value; }
        }
        public string DoctorName
        {
            get { return doctorName; }
            set { doctorName = value; }
        }

        public int Competence
        {
            get { return competence; }
            set { competence = value; }
        }

        public int Fatigue 
        {
            get { return fatigue; }
            set { fatigue = value; } 
        }

        public Doctor()
        {
            this.DoctorName = GenerateDoctorName();
            this.Competence = GenerateCompetence();
            this.Fatigue = 0;
        }
        public string GenerateDoctorName()
        {
            string[] names = { "Aron", "Rickard", "Henriette", "Katrin", "Adam", "Eva","Jeanette", "Erik", "Sara", "Eva", "Boris", "Lena", "Adam", "Adolfo", "Yngve", "Kerstin" };
            string[] lastNames = { "Andersson", "Olsson", "Bark", "Karlsson", "Nyqvist", "Hedlund", "Kozmakidis", "Billgren", "Niemon", "Keroi" };

            string firstName = names[rand.Next(0, names.Length)];
            string lastName = lastNames[rand.Next(0, lastNames.Length)];

            return firstName + " " + lastName;
        }
        public int GenerateCompetence()
        {
            int competence = rand.Next(-10, 30);
            return competence;
        }
    }
}
