using Krankenhaus.Backend;
using System;

namespace Krankenhaus
{
    public class Doctor : Person
    {
        Random rand = new Random((int)DateTime.Now.Ticks);

        private string doctorName;
        private int competence;
        private int fatigue;

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

        public Doctor(string name, int competence) : base (Title.Doctor)
        {
            this.DoctorName = name;
            this.Competence = competence;
            this.Fatigue = 0;
        }

        public Doctor() : base (Title.Doctor)
        {
            this.DoctorName = GenerateDoctorName();
            this.Competence = GenerateCompetence();
            this.Fatigue = 0;

        }

        private string GenerateDoctorName()
        {
            string[] names = { "Aron", "Rickard", "Henriette", "Katrin", "Adam", "Eva", "Jeanette", "Erik", "Sara", "Eva", "Boris", "Lena", "Adam", "Adolfo", "Yngve", "Kerstin", "Tintin", "Amanda", "Kenny", "Jennifer", "Frida", "Frodo", "Sam" };
            string[] lastNames = { "Andersson", "Olsson", "Bark", "Karlsson", "Nyqvist", "Hedlund", "Kozmakidis", "Billgren", "Niemon", "Keroi", "Petersson", "Boufadene", "Eriksson", "Henriksson", "Kristiansson", "Davidsson", "Godal", "Berg" };

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
            return $"Doctor#{this.DoctorName}#{this.Competence}#{this.Fatigue}";
        }
    }
}
