using System;
using System.Collections.Generic;
using System.IO;
namespace PigeonManagement
{
    public abstract class Pigeon
    {
        public string RingNumber { get; private set; }
        public string PigeonType { get; private set; }
        public string Breed { get; private set; }
        public string Color { get; private set; }
        public DateTime BirthDate { get; private set; }
        public bool IsAlive { get; private set; }

        public Pigeon(string ringNumber, string pigeonType, string breed, string color, DateTime birthDate, bool isAlive)
        {
            RingNumber = ringNumber;
            PigeonType = pigeonType;
            Breed = breed;
            Color = color;
            BirthDate = birthDate;
            IsAlive = isAlive;
        }

        public void UpdateStatus(bool status)
        {
            IsAlive = status;
        }



        protected void ShowDetails()
        {
            Console.WriteLine("Ring Number: " + RingNumber);
            Console.WriteLine("Pigeon Type: " + PigeonType);
            Console.WriteLine("Breed: " + Breed);
            Console.WriteLine("Color: " + Color);
            Console.WriteLine("Birth Date: " + BirthDate.ToString("MM/dd/yyyy"));

            if (IsAlive)
            {
                Console.WriteLine("Status: Alive");
            }
            else
            {
                Console.WriteLine("Status: Deceased");
            }
        }

        public virtual void DisplayInfo()
        {
            string status;

            if (IsAlive)
            {
                status = "Alive";
            }
            else
            {
                status = "Deceased";
            }

            Console.WriteLine("| {0,-11} | {1,-7} | {2,-18} | {3,-10} | {4,-12} | {5,-9}  |",
                RingNumber,
                PigeonType,
                Breed,
                Color,
                BirthDate.ToString("MM/dd/yyyy"),
                status);
        }
    }
    class RacingPigeon : Pigeon
    {
        public RacingPigeon(string ringNumber, string breed, string color, DateTime birthDate, bool isAlive)
            : base(ringNumber, "Racing", breed, color, birthDate, isAlive)
        {
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
        }
    }

    class FancyPigeon : Pigeon
    {
        public FancyPigeon(string ringNumber, string breed, string color, DateTime birthDate, bool isAlive)
            : base(ringNumber, "Fancy", breed, color, birthDate, isAlive)
        {
        }

        public override void DisplayInfo()
        {
            base.DisplayInfo();
        }
    }

    class PigeonManagement
    {
        private List<Pigeon> pigeons = new List<Pigeon>();

        public PigeonManagement()
        {
            LoadPigeons();
        }

        private void LoadPigeons()
        {

            string[] records = File.ReadAllLines("Pigeons.txt");

            foreach (string record in records)
            {
                string[] data = record.Split(',');

                if (data.Length == 6)
                {
                    DateTime birthDate;

                    if (DateTime.TryParse(data[4], out birthDate))
                    {

                        bool isAlive;

                        if (data[5] == "Alive")
                        {
                            isAlive = true;
                        }
                        else
                        {
                            isAlive = false;
                        }

                        if (data[1] == "Racing")
                        {
                            pigeons.Add(new RacingPigeon(
                                data[0], data[2], data[3], birthDate, isAlive));
                        }
                        else if (data[1] == "Fancy")
                        {
                            pigeons.Add(new FancyPigeon(
                                data[0], data[2], data[3], birthDate, isAlive));
                        }

                    }
                }
            }
        }

        private void SavePigeons()
        {
            List<string> records = new List<string>();

            foreach (Pigeon pigeon in pigeons)
            {
                string status;

                if (pigeon.IsAlive)
                {
                    status = "Alive";
                }
                else
                {
                    status = "Deceased";
                }

                string record = pigeon.RingNumber + "," +
                                pigeon.PigeonType + "," +
                                pigeon.Breed + "," +
                                pigeon.Color + "," +
                                pigeon.BirthDate.ToString("MM/dd/yyyy") + "," +
                                status;

                records.Add(record);
            }

            File.WriteAllLines("Pigeons.txt", records);
        }

        private void LogHistory(string action)
        {
            string record = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")
                            + " - " + action + "\n";

            File.AppendAllText("PigeonHistory.txt", record);
        }
        public void ViewHistory()
        {
            Console.Clear();

            

            if (!File.Exists("PigeonHistory.txt"))
            {
                Console.WriteLine("No history records found.");
                return;
            }

            string[] records = File.ReadAllLines("PigeonHistory.txt");

            if (records.Length == 0)
            {
                Console.WriteLine("No history records found.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("==============================================================");
            Console.WriteLine("|                     {0,-38} |", "History Record");
            Console.WriteLine("==============================================================");

            foreach (string record in records)
            {
                Console.WriteLine("| {0,-58} |", record);
            }
            Console.WriteLine("==============================================================");

        }



        private void DisplayPigeonTable(List<Pigeon> pigeonList)
        {
            Console.WriteLine();

            Console.WriteLine("=======================================================================================");
            Console.WriteLine("| {0,-11} | {1,-7} | {2,-18} | {3,-10} | {4,-12} | {5,-9}  |", "Ring Number", "Type", "Breed", "Color", "Birth Date", "Status");
            Console.WriteLine("=======================================================================================");

            foreach (Pigeon pigeon in pigeonList)
            {
                pigeon.DisplayInfo();
            }

          

        }
        private void DisplaySeparateTables()
        {
            List<Pigeon> racingPigeons = new List<Pigeon>();
            List<Pigeon> fancyPigeons = new List<Pigeon>();

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.PigeonType == "Racing")
                {
                    racingPigeons.Add(pigeon);
                }
                else if (pigeon.PigeonType == "Fancy")
                {
                    fancyPigeons.Add(pigeon);
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("|                                    {0,-47}  |", "RACING PIGEONS");
            if (racingPigeons.Count > 0)
            {
       
                DisplayPigeonTable(racingPigeons);
                Console.WriteLine("=======================================================================================");
                Console.WriteLine("Total Racing Pigeons: " + racingPigeons.Count) ;
                Console.WriteLine("=======================================================================================");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No Racing Pigeons found.");
            }
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.Write("|                                    {0,-47}  |", "FANCY PIGEONS");
            if (fancyPigeons.Count > 0)
            {
                DisplayPigeonTable(fancyPigeons);
                Console.WriteLine("=======================================================================================");
                Console.WriteLine("Total Fancy Pigeons: " + fancyPigeons.Count);
                Console.WriteLine("=======================================================================================");
            }
            else
            {
                Console.WriteLine("No Fancy Pigeons found.");
            }
        }
        public void ViewPigeons()
        {
            Console.Clear();

            if (pigeons.Count == 0)
            {
                Console.WriteLine("No pigeon records found.");
                return;
            }
            int alivecount = 0;
            int deadcount = 0;
            foreach (Pigeon p in pigeons)
            {
                if(p.IsAlive)
                {
                    alivecount++;
                }
                else
                {
                    deadcount++;
                }
                
            }
            DisplaySeparateTables();
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("\nTotal Alive Pigeons: " + alivecount);
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("\nTotal Dead Pigeons: " + deadcount);
            Console.ResetColor();


        }


        private Pigeon GetPigeonDetails(string ringNumber)
        {
            Console.Write("Enter Breed: ");
            string breed = Console.ReadLine();

            Console.WriteLine("Select Pigeon Type:");
            Console.WriteLine("1. Racing Pigeon");
            Console.WriteLine("2. Fancy Pigeon");
            Console.Write("Enter choice: ");

            string typeChoice = Console.ReadLine();

            if (typeChoice != "1" && typeChoice != "2")
            {
                Console.WriteLine("Invalid pigeon type.");
                return null;
            }

            Console.Write("Enter Color: ");
            string color = Console.ReadLine();

            Console.Write("Enter Birth Date (MM/DD/YYYY): ");
            string dateInput = Console.ReadLine();

            DateTime birthDate;

            if (!DateTime.TryParse(dateInput, out birthDate))
            {
                Console.WriteLine("Invalid birth date.");
                return null;
            }


            Console.WriteLine("Select Status:");
            Console.WriteLine("1. Alive");
            Console.WriteLine("2. Deceased");
            Console.Write("Enter choice: ");

            string statusChoice = Console.ReadLine();

            bool isAlive;

            if (statusChoice == "1")
            {
                isAlive = true;
            }
            else if (statusChoice == "2")
            {
                isAlive = false;
            }
            else
            {
                Console.WriteLine("Invalid status.");
                return null;
            }

            if (typeChoice == "1")
            {
                return new RacingPigeon(ringNumber, breed, color, birthDate, isAlive);
            }
            else
            {
                return new FancyPigeon(ringNumber, breed, color, birthDate, isAlive);
            }
        }

        public void AddPigeon()
        {
            Console.Clear();

            Console.WriteLine("========== ADD PIGEON ==========");

            Console.Write("Enter Ring Number: ");
            string ringNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(ringNumber))
            {
                Console.WriteLine("Ring Number cannot be empty.");
                return;
            }

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber == ringNumber)
                {
                    Console.WriteLine("Ring Number already exists.");
                    return;
                }
            }

            Pigeon newPigeon = GetPigeonDetails(ringNumber);

            if (newPigeon != null)
            {
                pigeons.Add(newPigeon);

                SavePigeons();
                LogHistory("Added pigeon: " + ringNumber);

                Console.WriteLine("\nPigeon successfully added!");
            }
        }



        public void SearchPigeon()
        {
            Console.Clear();

            Console.WriteLine("========== SEARCH PIGEON ==========");

            Console.Write("Enter Ring Number, PigeonType, Color, or Status: ");
            string searchValue = Console.ReadLine();


            List<Pigeon> searchResults = new List<Pigeon>();

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber.ToLower() == searchValue.ToLower() ||
                    pigeon.PigeonType.ToLower() == searchValue.ToLower() ||
                    pigeon.Color.ToLower() == searchValue.ToLower() ||
                    (pigeon.IsAlive && searchValue.ToLower() == "alive") ||
                    (!pigeon.IsAlive && searchValue.ToLower() == "deceased"))
                {
                    searchResults.Add(pigeon);
                }
            }


            if (searchResults.Count == 0)
            {
                Console.WriteLine("\nPigeon not found.");
            }
            else
            {
                DisplayPigeonTable(searchResults);
            }
        }

        public void UpdatePigeon()
        {
            Console.Clear();
            DisplayPigeonTable(pigeons);
            Console.WriteLine("========== EDIT PIGEON ==========");

            Console.Write("Enter Ring Number of pigeon to edit: ");
            string oldRingNumber = Console.ReadLine();

            Pigeon foundPigeon = null;

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber == oldRingNumber)
                {
                    foundPigeon = pigeon;
                    break;
                }
            }

            if (foundPigeon == null)
            {
                Console.WriteLine("Pigeon not found.");
                return;
            }

            Console.Write("\nEnter New Ring Number: ");
            string newRingNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(newRingNumber))
            {
                Console.WriteLine("Ring Number cannot be empty.");
                return;
            }

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber == newRingNumber && pigeon != foundPigeon)
                {
                    Console.WriteLine("Ring Number already exists.");
                    return;
                }
            }

            Pigeon updatedPigeon = GetPigeonDetails(newRingNumber);

            if (updatedPigeon != null)
            {
                for (int i = 0; i < pigeons.Count; i++)
                {
                    if (pigeons[i] == foundPigeon)
                    {
                        pigeons[i] = updatedPigeon;
                        break;
                    }
                }

                SavePigeons();
                LogHistory("Updated pigeon: " + oldRingNumber + " to " + newRingNumber);

                Console.WriteLine("\nPigeon successfully updated!");
            }
        }

        public void ChangeStatus()
        {
            Console.Clear();
            DisplayPigeonTable(pigeons);
            Console.WriteLine("========== CHANGE PIGEON STATUS ==========");

            Console.Write("Enter Ring Number: ");
            string ringNumber = Console.ReadLine();

            Pigeon foundPigeon = null;

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber == ringNumber)
                {
                    foundPigeon = pigeon;
                    break;
                }
            }

            if (foundPigeon == null)
            {
                Console.WriteLine("Pigeon not found.");
                return;
            }

            Console.WriteLine("\nCurrent Status:");

            if (foundPigeon.IsAlive)
            {
                Console.WriteLine("Alive");
            }
            else
            {
                Console.WriteLine("Deceased");
            }

            Console.WriteLine("\nSelect New Status:");
            Console.WriteLine("1. Alive");
            Console.WriteLine("2. Deceased");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                foundPigeon.UpdateStatus(true);
            }
            else if (choice == "2")
            {
                foundPigeon.UpdateStatus(false);
            }
            else
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            SavePigeons();
            LogHistory("Changed status of pigeon: " + ringNumber);

            Console.WriteLine("\nPigeon status successfully updated!");
        }
        public void DeletePigeon()
        {
            Console.Clear();
            DisplayPigeonTable(pigeons);
            Console.WriteLine("========== DELETE PIGEON ==========");

            Console.Write("Enter Ring Number: ");
            string ringNumber = Console.ReadLine();

            Pigeon foundPigeon = null;

            foreach (Pigeon pigeon in pigeons)
            {
                if (pigeon.RingNumber == ringNumber)
                {
                    foundPigeon = pigeon;
                    break;
                }
            }

            if (foundPigeon == null)
            {
                Console.WriteLine("Pigeon not found.");
                return;
            }

            Console.Write("\nAre you sure you want to delete this pigeon? (Y/N): ");
            string confirmation = Console.ReadLine();

            if (confirmation == "Y" || confirmation == "y")
            {
                pigeons.Remove(foundPigeon);

                SavePigeons();
                LogHistory("Deleted pigeon: " + ringNumber);

                Console.WriteLine("\nPigeon successfully deleted!");
            }
            else
            {
                Console.WriteLine("\nDeletion cancelled.");
            }
        }
    }
    class Menu
    {
        public void ManageMenu(PigeonManagement management)
        {
            string choice = "";

            while (choice != "6")
            {
                Console.Clear();

                Console.WriteLine("========== MANAGE PIGEONS ==========");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[1]. Add Pigeon");
                Console.WriteLine("[2]. Edit Pigeon");
                Console.WriteLine("[3]. Search Pigeon");
                Console.WriteLine("[4]. Change Pigeon Status");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[5]. Delete Pigeon");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[6]. Back to Main Menu");

                Console.Write("\nEnter your choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        management.AddPigeon();
                        break;

                    case "2":
                        management.UpdatePigeon();
                        break;

                    case "3":
                        management.SearchPigeon();
                        break;

                    case "4":
                        management.ChangeStatus();
                        break;

                    case "5":
                        management.DeletePigeon();
                        break;

                    case "6":
                        Console.WriteLine("Returning to Main Menu...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }

                if (choice != "6")
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                }
            }
        }
    }
    internal class Program
    {
        static void Main()
        {
            PigeonManagement management = new PigeonManagement();
            Menu menu = new Menu();

            string choice = "";

            while (choice != "4")
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine(@"
                    
____________    __      ____________
\_____     /   /_ \     \     _____/
 \_____    \____/  \____/    _____/
  \_____                    _____/
     \___________  ___________/
               /____\
");
                Console.ForegroundColor = ConsoleColor.White;   
                Console.WriteLine("==================================");
                Console.WriteLine("       PIGEON MANAGEMENT SYSTEM");
                Console.WriteLine("==================================");

                Console.WriteLine("[1]. Show All Pigeons");
                Console.WriteLine("[2]. Manage Pigeons");
                Console.WriteLine("[3]. View System History");
                Console.WriteLine("[4]. Exit");

                Console.Write("\nEnter your choice: ");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        management.ViewPigeons();

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        menu.ManageMenu(management);
                        break;

                    case "3":
                        management.ViewHistory();

                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadKey();
                        break;

                    case "4":
                        Console.WriteLine("\nThank you for using the Pigeon Management System!");
                        break;

                    default:
                        Console.WriteLine("\nInvalid choice.");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}
