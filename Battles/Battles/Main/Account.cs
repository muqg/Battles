using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Battles
{
    static class Account
    {
        private const int nameLength = 4;
        private const int passLength = 6;

        public static Player Login()
        {
            while (true)
            {
                Console.WriteLine($"Enter your username {Constants.BackMessage}:");
                string name = Command.ParseAlphaNumericCommand();
                if (name.Equals(Constants.BackCommand)) // Check for back command
                    return null;

                string filename = Directory.GetCurrentDirectory() + $"/{name}.bin";
                if (File.Exists(filename)) // Check if save file exists
                {
                    // Password loop
                    while(true)
                    {
                        Console.WriteLine($"Enter your password {Constants.BackMessage}:");
                        string pass = Command.ParseCommand();
                        if (pass.Equals(Constants.BackCommand)) // Check for back command
                            break;
                        else // Load file and check if password matches
                        {
                            using (FileStream fs = File.Open(filename, FileMode.Open)) // Load the file
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                Player player = bf.Deserialize(fs) as Player;

                                if (!player.CheckPassword(pass)) // Password does not match
                                {
                                    Console.WriteLine("Invalid password.\n");
                                    continue;
                                }

                                // Password matches
                                player.Load();
                                return player;
                            } 
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Username does not exist.\n");
                    continue;
                }
            }
        }

        public static Player New()
        {
            while (true)
            {
                string name = createUsername();
                if (name.Equals(Constants.BackCommand)) // Check for back command
                    return null;

                string pass = createPassword();
                if (pass.Equals(Constants.BackCommand)) // Check for back command
                    continue;

                Player player = new Player(name, pass);
                player.Save(); // Save the newly created player account
                return player;
            }
        }

        private static string createUsername()
        {
            while (true)
            {
                Console.WriteLine($"Enter your username {Constants.BackMessage}:");
                string name = Command.ParseAlphaNumericCommand();

                if (name.Length < nameLength && !name.Equals(Constants.BackCommand)) // Restrictions and back command
                {
                    Console.WriteLine($"Username must be at least {nameLength}-character long including only letters and numbers.\n");
                    continue;
                }
                else if (File.Exists(Directory.GetCurrentDirectory() + $"/{name}.bin")) // Check if save file already exists
                {
                    Console.WriteLine("Player with that name already exists. Login instead.\n");
                    continue;
                }

                return name;
            }
        }

        private static string createPassword()
        {
            while (true)
            {
                Console.WriteLine($"Enter your password {Constants.BackMessage}:");
                string pass = Command.ParseCommand();

                if (pass.Length < passLength && !pass.Equals(Constants.BackCommand)) // Restrictions and back command
                {
                    Console.WriteLine($"Password must be at least {passLength} characters long.\n");
                    continue;
                }
                return pass;
            }
        }
    }
}
