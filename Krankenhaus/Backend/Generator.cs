using Krankenhaus.Backend;
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
        private Queue queue;
        private Sanatorium sanatorium;
        private IVA iva;
        private Ticker ticker;
        private Frontend menu;
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
            menu = new Frontend();
            logger = new Logger();
        }

        public void Start()
        {
            ticker.Tick += queue.OnTick;
            StartClock += StartTicker;
            ticker.Tick += StatusReport;
            ticker.Tick += CheckIfPatientsExist;
            ticker.Tick += sanatorium.OnTick;
            ticker.Tick += iva.OnTick;
            UpdateStatus += SaveToFile;
            ticker.Tick += survivors.OnTick;
            ticker.Tick += afterlife.OnTick;
            ticker.TickerStop += menu.DisplayResult;

            ticker.TickerStop += iva.ClearDoctorsFile;
            ticker.TickerStop += afterlife.ClearFile;
            ticker.TickerStop += survivors.ClearFile;

            FileReading += sanatorium.ReadData;
            FileReading += iva.ReadData;
            FileReading += iva.ReadDoctors;
            FileReading += afterlife.ReadData;
            FileReading += survivors.ReadData;
            //FileReading += queue.ReadData;


            if (menu.ReadData(out int ticksReturn))
            {
                ticks = ticksReturn;
                FileReading?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                int doctorInput = menu.DoctorInput();
                int patientInput = menu.PatientInput();

                MakePatients(patientInput);
                Console.Clear();

                ShowQueue();

                iva.MakeDoctors(doctorInput);
            }

            StartClock?.Invoke(this, EventArgs.Empty);
        }

        private async void StartTicker(object sender, EventArgs e)
        {
            int time = menu.Menu();
            await Task.Run(() => ticker.Ticking(time));
        }

        public void ShowQueue()
        {
            Console.WriteLine(queue.GetAllPatients());
        }

        private async void CheckIfPatientsExist(object sender, EventArgs e)
        {
            if (queue.Length == 0)
            {
                if (sanatorium.OccupiedBeds == 0 && iva.OccupiedBeds == 0)
                {
                    await ticker.StopTick();
                }
            }
        }
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

            
            UpdateStatus?.Invoke(this, new UpdateStatusArgs(sb.ToString()));
            await logger.LogToFile("Ticker.txt", ticker.tick.ToString(), false);
            await FillHospital();
        }

        private async void SaveToFile(object sender, UpdateStatusArgs e)
        {
            await logger.LogToFile(fileName, e.Status, true);
        }

        private async Task FillHospital()
        {
            while (queue.Length != 0)
            {
                if (!queue.Peek().IsAlive)
                {
                    while (afterlife.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    afterlife.Add(queue.GetNextPatient());
                    continue;
                }

                if (!iva.IsFull)
                {
                    while (iva.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    iva.CheckIn(queue.GetNextPatient());
                    
                }
                else if (!sanatorium.IsFull)
                {
                    while (sanatorium.Saving || queue.Saving)
                    {
                        await Task.Delay(1);
                    }
                    sanatorium.CheckIn(queue.GetNextPatient());
                }
                else
                {
                    break;
                }
            }

            
        }

        private void MakePatients(int userInput)
        {
            for (int i = 0; i < userInput; i++)
            {
                Patient patient = new Patient();
                queue.AddToQueue(patient);
                Thread.Sleep(250);
            }
        }
    }
}
