using System;
using System.Text.RegularExpressions;

namespace Battles
{
    static class Command
    {
        // Parses a command including a single number
        public static int ParseIntCommand()
        {
            string command = getCommand(Constants.IntCommandPattern);
            if (command.Length > 0)
            {
                return int.Parse(command);
            }
            else
            {
                return 0;
            }
        }

        // Parses a command including letters
        public static string ParseAlphaCommand()
        {
            return getCommand(Constants.AlphaCommandPattern);
        }

        // Parses a command including numbers and letters
        public static string ParseAlphaNumericCommand()
        {
            return getCommand(Constants.AlphaNumbericCommandPattern);
        }

        // Parses a command including numbers, letters and symbols
        public static string ParseCommand()
        {
            return getCommand(Constants.CommandPattern);
        }

        // Gets the command itself
        private static string getCommand(string pattern)
        {
            Match m = Regex.Match(Console.ReadLine(), pattern);
            Console.WriteLine();
            return m.Value;
        }
    }
}
