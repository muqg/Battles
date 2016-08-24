using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Battles
{
    static class Utility
    {
        private static Random random = new Random();

        public static string GetPasswordHash(string password, byte[] salt)
        {
            using (SHA512CryptoServiceProvider sha = new SHA512CryptoServiceProvider())
            {
                byte[] hashBytes = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
                sha.ComputeHash(hashBytes);
                return Convert.ToBase64String(sha.Hash);
            }
        }

        public static Type GetType(string className)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .First(t => t.Name == className);
        }

        public static Type[] GetTypes(string nameSpace)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && t.Namespace == nameSpace).ToArray();
        }

        public static bool ConfirmAction()
        {
            while(true)
            {
                Console.WriteLine("Are you sure? This cannot be undone!\n1. Yes\n2. No");

                int command = Command.ParseIntCommand();
                if (command == 1)
                    return true;
                else if (command == 2)
                    return false;

                Console.WriteLine(Constants.InvalidCommand);
            }
        }

        public static bool GetPseudoChance(int defaultChance, ref int pseudoChance)
        {
            if(random.Next(0, 100) < pseudoChance)
            {
                pseudoChance = defaultChance;
                return true;
            }

            pseudoChance += Math.Max(1, defaultChance / 2); // Limit minimum increase to 1
            return false;
        }

        #region Extensions

        public static int Clamp(this int val, int min, int max) => Math.Min(max, Math.Max(min, val));

        public static float Clamp(this float val, float min, float max) => Math.Min(max, Math.Max(min, val));

        public static string SplitCapital(this string str) => Regex.Replace(str, "([a-z])([A-Z])", "$1 $2");

        public static string Indent(this string str, int count = 8) => $"{"".PadLeft(count)}{str}";

        #endregion Extensions
    }
}
