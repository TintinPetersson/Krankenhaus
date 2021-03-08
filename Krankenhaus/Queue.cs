using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Queue
    {
        private Queue<Patient> patients;
        Random rand = new Random();

        
        public Queue()
        {
            patients = new Queue<Patient>();
        }

        public int Length { get => patients.Count; }

        public Patient GetNextPatient()
        {
            //try catch
            return patients.Dequeue();
        }

        public void AddToQueue(Patient patient)
        {
            //try catch
            patients.Enqueue(patient);
        }

        public void ChangeSicknessLevel()
        {
            foreach(Patient patient in patients)
            {
                int startingSickness = rand.Next(1, 9);
                patient.SicknessLevel = startingSickness;
            }
        }

        public void OnTick(object sender, EventArgs e)
        {
            foreach (Patient patient in patients)
            {
                int newSickness = rand.Next(1, 21);

                if (newSickness == 1)
                {
                    newSickness = patient.SicknessLevel - 1;
                }
                else if(newSickness <= 17)
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

        public string GetAllPatients()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Patient patient in patients)
            {
                sb.AppendLine($"\tPatient: {patient.PatientName.PadLeft(5).PadRight(20)} " +
                    $"| Sickness level : {patient.SicknessLevel.ToString().PadLeft(2).PadRight(5)} | " +
                    $"Date of Birth : {patient.DateOfBirth.PadLeft(5).PadRight(15)} | Alive: {patient.IsAlive.ToString().PadLeft(5)}");
            }
            return sb.ToString();
        }
    }
}
