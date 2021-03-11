using Krankenhaus.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Patient : Person
    {
        Random rand = new Random((int)DateTime.Now.Ticks);
        public DateTime ArrivalToHospital { get; set; }
        public DateTime DepartureFromHospital { get; set; }
        public string PatientName { get; set; }
        public string DateOfBirth { get; private set; }
        public int SicknessLevel { get; set; }
        public bool IsAlive { get => this.SicknessLevel < 10; }

        public Patient(string name, int sickness, string birth, DateTime arrival, DateTime departure) : base (Title.Patient)
        {
            this.PatientName = name;
            this.SicknessLevel = sickness;
            this.DateOfBirth = birth;
            this.ArrivalToHospital = arrival;
            this.DepartureFromHospital = departure;

        }

        public Patient() : base(Title.Patient)
        {
            this.PatientName = GeneratePatientName();
            this.DateOfBirth = GenerateBirthDate();
            this.SicknessLevel = GenerateSicknessLevel();
        }

        
        public string GeneratePatientName()
        {
            string[] names = { "Elias", "Karin", "Ola", "Henrik", "Adolf", "Peter", "Oskar", "Olga", "Pontus", 
                "Svetlana", "Bill", "Meali", "Jon", "Tryggve", "Lisa", "Nedrin", "Conny", "Sonny", "Berra",
                "Ilfa", "Anders", "Lars", "Robin", "Oskar", "Jonte", "Dessi", "Yumit", "Emanuel", "André" };
            string[] lastNames = { "Andersson", "Olsson", "Brugg", "Karlsson", "Lokran", "Ergan", "Persson", "Bandera", "Nilsson", "Skogh", "Tillerås", "Guyler", 
            "Kullman", "Tannenberg", "Robinsson", "Petersson", "Eriksson", "Berg", "Baggins", "Boufadene", "Gamgee", "Johansson", "Hansson"};

            string firstName = names[rand.Next(0, names.Length)];
            string lastName = lastNames[rand.Next(0, lastNames.Length)];

            return firstName + " " + lastName;
        }
        public int GenerateSicknessLevel()
        {
            int startingSickness = rand.Next(1, 9);
            return startingSickness;
        }

        public string GenerateBirthDate()
        {
            DateTime start = new DateTime(1925, 1, 1);
            DateTime end = new DateTime(2005, 12, 30);
            int range = (end - start).Days;

            string date = start.AddDays(rand.Next(range)).ToShortDateString();

            return date;
        }

        public override string ToString()
        {
            return $"Patient#{this.PatientName}#{this.SicknessLevel}#{this.DateOfBirth}#{this.IsAlive}" +
                $"#{this.ArrivalToHospital}#{this.DepartureFromHospital}";
        }
    }
}
