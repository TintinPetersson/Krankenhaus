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
            private set { doctorId = value; }
        }
        public string DoctorName
        {
            get { return doctorName; }
            private set { doctorName = value; }
        }
        public int Competence
        {
            get { return competence; }
            private set { competence = value; }
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
        private string GenerateDoctorName()
        {
            string[] names = { "Aron", "Rickard", "Henriette", "Katrin", "Adam", "Eva", "Jeanette", "Erik", "Sara", "Eva", "Boris", "Lena", "Adam", "Adolfo", "Yngve", "Kerstin" };
            string[] lastNames = { "Andersson", "Olsson", "Bark", "Karlsson", "Nyqvist", "Hedlund", "Kozmakidis", "Billgren", "Niemon", "Keroi" };

            string firstName = names[rand.Next(0, names.Length)];
            string lastName = lastNames[rand.Next(0, lastNames.Length)];

            return firstName + " " + lastName;
        }
        private int GenerateCompetence()
        {
            int competence = rand.Next(-2, 6);
            return competence;
        }

        public override string ToString()
        {
            return $"#{this.DoctorName}#{this.Competence}#{this.Fatigue}";
        }
    }
}
