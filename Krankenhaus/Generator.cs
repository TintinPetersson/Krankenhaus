using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Generator
    {
        public static void Start()
        {

            Queue<Doctor> doctors = MakeDoctors();
            Queue<Patient> patients = MakePatients();

            Ticker ticker = new Ticker();

            foreach (Doctor doc in doctors)
            {
                Console.WriteLine($"\tDoctor: {doc.DoctorName.PadLeft(5).PadRight(20)} | Competence : " +
                    $"{doc.Competence.ToString().PadLeft(1).PadRight(10)} | Fatigue : {doc.Fatigue.ToString().PadLeft(1).PadRight(5)} ");
            }

            Console.WriteLine("-------------------------------");

            foreach (Patient patient in patients)
            {
                Console.WriteLine($"\tPatient: {patient.PatientName.PadLeft(5).PadRight(20)} " +
                    $"| Sickness level : {patient.SicknessLevel.ToString().PadLeft(2).PadRight(5)} | " +
                    $"Date of Birth : {patient.DateOfBirth.PadLeft(5).PadRight(15)} | Alive: {patient.IsAlive.ToString().PadLeft(5)}");
            }

        }
        public static Queue<Doctor> MakeDoctors()
        {
            Queue<Doctor> doctors = new Queue<Doctor>();

            for (int i = 0; i < 10; i++)
            {

                Doctor doctor = new Doctor();
                doctors.Enqueue(doctor);
                Thread.Sleep(250);
            }
            return doctors;
        }
        public static Queue<Patient> MakePatients()
        {
            Queue<Patient> patients = new Queue<Patient>();

            for (int i = 0; i < 25; i++)
            {

                Patient patient = new Patient();
                patients.Enqueue(patient);
                Thread.Sleep(250);
            }
            return patients;
        }
    }
}
