using System;

namespace FileSystemTPL
{
    class Program
    {
        public const string DIRECTORY_PATH = @"C:\Users\itayl\OneDrive\שולחן העבודה\trial_folder";
        public const int DELAY_MS = 1000;
        public const string DEFAULT_COMMANDS_PATH = @"..\..\..\commands.txt";

        static void Main()
        {
            CommandExecutor commandExecutor = new CommandExecutor(DELAY_MS);

            while (true)
            {
                Console.WriteLine("Press 1 for executing file commands.\nPress 2 for executing console commands.");
                string choice = Console.ReadLine();

                if (choice.Trim() == "1")
                {
                    commandExecutor.RunCommandsFromFile();
                    return;
                }
                else if (choice.Trim() == "2") 
                {
                    commandExecutor.GetUserCommands();
                    return;
                }
            }
        }
    }
}
