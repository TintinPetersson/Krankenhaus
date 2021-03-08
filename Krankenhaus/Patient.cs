using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Patient
    {

        Random rand = new Random();

        public int PatientID { get; private set; }

        public string PatientName { get; set; }

        public string DateOfBirth { get; private set; }

        public int SicknessLevel { get; set; }

        public bool IsAlive { get; set; }

        public Patient()
        {
            this.PatientID = 0;
            this.PatientName = GeneratePatientName();
            this.IsAlive = true;
            this.DateOfBirth = GenerateBirthDate();
            this.SicknessLevel = GenerateSicknessLevel();
        }

        
        public string GeneratePatientName()
        {
            string[] names = { "Elias", "Karin", "Ola", "Henrik", "Peter", "Oskar", "Olga", "Pontus", "Svetlana", "Bill", "Meali", "Jon", "Tryggve", "Lisa", "Nedrin", "Ilfa", "Anders" };
            string[] lastNames = { "Andersson", "Olsson", "Brugg", "Karlsson", "Lokran", "Ergan", "Persson", "Bandera", "Nilsson", "Skogh" };

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
    }
}
