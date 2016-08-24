using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

namespace Battles
{
    [Serializable]
    sealed class Player
    {
        private string _password;

        public Player(string username, string password)
        {
            Name = username;
            Password = password;
        }

        public string Name { get; }
        public List<Character> Characters { get; } = new List<Character>(Constants.MaxCharacters);

        private byte[] Salt { get; set; } = new byte[16];
        private string Password
        {
            get
            {
                return _password;
            }
            set
            {
                using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(Salt); // Always update salt on password change
                    _password = Utility.GetPasswordHash(value, Salt); // Store hashed password and not real password
                    Save(); // Save player data on password change
                }
            }
        }

        // Loads all constants and variables anew for if anything has changed
        // Should prevent and handle deserialization errors morroring any changes and adding new parts to the object flawlessly
        public void Load()
        {
            Console.WriteLine("Loading...");

            int characterCount = Characters.Count;
            int percentage = 100 / (characterCount == 0 ? 1 : characterCount);
            for (int i = 0; i < characterCount; i++)
            {
                Characters[i] = Character.Load(Characters[i]);
                Console.WriteLine($"{(i + 1) * percentage}%");
            }

            Console.WriteLine($"{Name} save file loaded.\n");
        }

        // Saves the player profile as serialized binary file
        public void Save()
        {
            string filename = Directory.GetCurrentDirectory() + $"/{Name}.bin";
            BinaryFormatter bf = new BinaryFormatter();

            using (FileStream fs = File.Open(filename, FileMode.OpenOrCreate))
            {
                bf.Serialize(fs, this);
            }
        }

        // Checks wether pass matches the players' and returns true or false
        public bool CheckPassword(string pass) => Password.Equals(Utility.GetPasswordHash(pass, Salt));
    }
}
