using System;

namespace Battles
{
    static class Game
    {
        static MainMenu mainMenu = new MainMenu();

        public static bool IsRunning { get; set; } = true;

        public static Player CurrentPlayer { get; set; } = null;
        public static Character CurrentCharacter { get; set; } = null;
        public static Skill CurrentSkill { get; set; } = null;
        public static Item CurrentItem { get; set; } = null;

        static void Main(string[] args)
        {
            Console.Title = "Project: Battles";

            while (IsRunning)
            {
                mainMenu.Show();
                CurrentPlayer = null;
            }

            CurrentPlayer?.Save();
            Console.WriteLine("Goodbye.\nPress any key to close.");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
