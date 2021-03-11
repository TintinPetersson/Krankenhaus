using Krankenhaus.Backend;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Generator
    {
        private Queue queue;
        private Sanatorium sanatorium;
        private IVA iva;
        private Ticker ticker;
        private Frontend frontend;
        private Logger logger;
        private AfterLife afterlife;
        private Survivors survivors;

        private EventHandler<EventArgs> StartClock;
        public static EventHandler<UpdateStatusArgs> UpdateStatus;
        public EventHandler<EventArgs> FileReading;

        private string fileName;
        private int ticks = 0;
        
        public Generator()
        {
            fileName = "KrankenhausReport.txt";
            queue = new Queue();
            afterlife = AfterLife.GetInstance();
            ticker = new Ticker();
            sanatorium = new Sanatorium();
            iva = new IVA();
            survivors = Survivors.GetInstance();
            frontend = new Frontend();
            logger = new Logger();
        }

        public void Start()
        {
            #region Prenumerationer
            StartClock += StartTicker;

            ticker.Tick += queue.OnTick;
            ticker.Tick += StatusReport;
            ticker.Tick += CheckIfPatientsLeft;
            ticker.Tick += sanatorium.OnTick;
            ticker.Tick += iva.OnTick;
            ticker.Tick += survivors.OnTick;
            ticker.Tick += afterlife.OnTick;

            UpdateStatus += SaveToFile;

            ticker.TickerStop += frontend.DisplayResult;
            ticker.TickerStop += iva.ClearDoctorsFile;
            ticker.TickerStop += afterlife.ClearFile;
            ticker.TickerStop += survivors.ClearFile;

            FileReading += sanatorium.ReadData;
            FileReading += iva.ReadData;
            FileReading += iva.ReadDoctors;
            FileReading += afterlife.ReadData;
            FileReading += survivors.ReadData;
            FileReading += queue.ReadData;
            #endregion

            // Checks if there already exist files to read from
            if (frontend.ReadData(out int ticksReturn))
            {
                ticks = ticksReturn;
                FileReading?.Invoke(this, EventArgs.Empty);
            }
            // If there are no files or the user doesn't want to read from existing files
            else
            {
                // Gets the user's input about how many doctors and patients should be generated
                int doctorInput = frontend.DoctorInput();
                int patientInput = frontend.PatientInput();

                // Genereate patients
                MakePatients(patientInput);
                Console.Clear();

                // Displays the generated patients in the queue
                ShowQueue();

                // Generates doctors for IVA
                iva.MakeDoctors(doctorInput);
            }

            StartClock?.Invoke(this, EventArgs.Empty);
        }

        private async void StartTicker(object sender, EventArgs e)
        {
            int time = frontend.GetSpeed();
            await Task.Run(() => ticker.Ticking(time));
        }

        public void ShowQueue()
        {
            frontend.DisplayList<Patient>(queue.GetAllPatients());
        }

        /// <summary>
        /// Checks every tick if there are any patients left in the hospital, 
        /// if not it stops the ticker
        /// </summary>
        private async void CheckIfPatientsLeft(object sender, EventArgs e)
        {
            if (queue.Length == 0)
            {
                if (sanatorium.OccupiedBeds == 0 && iva.OccupiedBeds == 0)
                {
                    await ticker.StopTick();
                }
            }
        }

        /// <summary>
        /// Updates status each tick with a specific format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void StatusReport(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Tick: {ticker.tick + ticks}");
            sb.AppendLine($"Queue: {queue.Length} patients");
            sb.AppendLine($"Sanatorium: {sanatorium.OccupiedBeds} patients");
            sb.AppendLine($"IVA: {iva.OccupiedBeds} patients");
            sb.AppendLine($"Doctor Present: {iva.IsDoctorPresent}");
            sb.AppendLine($"Afterlife: {afterlife.Length}");
            sb.AppendLine($"Survivors: {survivors.Length}");

            // Invokes an event that sends out the status report
            UpdateStatus?.Invoke(this, new UpdateStatusArgs(sb.ToString()));

            // Saves to file which tick we are on
            await logger.LogToFile("Ticker.txt", ticker.tick.ToString(), false);

            // Retrieves patients from queue and adds them to the hospital
            await FillHospital();
        }

        /// <summary>
        /// Saves the status report to a file
        /// </summary>
        private async void SaveToFile(object sender, UpdateStatusArgs e)
        {
            await logger.LogToFile(fileName, e.Status, true);
        }

        /// <summary>
        /// Retrieves patients from queue and adds them to IVA, Sanatorium,
        /// or Survivors if their sickness level is 0, or to Afterlife if they are dead
        /// </summary>
        private async Task FillHospital()
        {
            while (queue.Length != 0)
            {
                if (!queue.Peek().IsAlive) // Adds the first person in line to Afterlife if it is dead
                {
                    while (afterlife.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    afterlife.Add(queue.GetNextPatient());
                    continue;
                }
                else if (queue.Peek().SicknessLevel == 0) // Adds the first person in line to survive if it has already recovered
                {
                    while (survivors.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    survivors.Add(queue.GetNextPatient());
                    continue;
                }

                if (!iva.IsFull) // Adds the first person in line to IVA if there is room
                {
                    while (iva.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    iva.CheckIn(queue.GetNextPatient());
                    
                }
                else if (!sanatorium.IsFull) // Adds the first person in line to Sanatorium if there is room
                {
                    while (sanatorium.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    sanatorium.CheckIn(queue.GetNextPatient());
                }
                else // If both IVA and Sanatorium are full no more patients can be reitreved from the queue
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Generates patients with random names and sickness level
        /// </summary>
        /// <param name="userInput">Number of patients to be generated</param>
        private void MakePatients(int userInput)
        {
            for (int i = 0; i < userInput; i++)
            {
                Patient patient = new Patient();
                queue.AddToQueue(patient);
                // Since random is based on DateTime we have to use Thread.Sleep
                // to get variation in names and sickness level
                Thread.Sleep(250); 
            }
        }
    }
}
