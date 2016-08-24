using System;
using System.Collections.Generic;

namespace Battles
{
    abstract class Menu
    {
        protected string menuName;
        protected string[] options;
        protected Action[] actions;

        // Initialize options to the static initialization variable and actions to the apropriate length
        public Menu(string[] optionsInit, string name = "")
        {
            menuName = name;
            options = optionsInit;
            actions = new Action[options.Length - 1];
        }

        public static void Announce(string message)
        {
            Console.WriteLine($"--- {message} ---");
        }

        // Shows all available options to choose from
        public static void ShowOptions(string[] options, bool showBack = false)
        {
            int i;
            for (i = 0; i < options.Length; i++)
            {
                Console.WriteLine(i + 1 + ". " + options[i]);
            }

            if (showBack)
                Console.WriteLine($"{i + 1}. Back");
        }

        public static void ShowSingleActionMenu<T>(List<T> objects, Action<T> action, string menuName = "", string zeroErrorMessage = "") where T : ISingleActionMenu
        {
            // No objects in list
            if(objects.Count == 0)
            {
                if (zeroErrorMessage.Length > 0)
                    Console.WriteLine(zeroErrorMessage + "\n");
                return;
            }

            int optionsLength = objects.Count + 1;
            string[] options = new string[optionsLength];
            for (int i = 0; i < objects.Count; i++)
                options[i] = objects[i].ToString();
                
            options[optionsLength - 1] = "Back";

            if (menuName.Length > 0)
                Announce(menuName);

            ShowOptions(options);

            int command = Command.ParseIntCommand();
            if (command > 0 && command < optionsLength)
            {
                try
                {
                    int j = command - 1;
                    action(objects[j]);
                }
                catch (Exception ex) when (ex is NullReferenceException || ex is NotImplementedException) // Should there be more options than actions available
                {
                    Console.WriteLine("Option is Not Yet Implemented (NYI)\n");
                }
            }
            else if (command == optionsLength) // Last option is always "exit" or "back"
            {
                return;
            }
            else
                Console.WriteLine("Invalid Command.\n");
        }

        public void Show()
        {
            while(true)
            {
                if (menuName.Length > 0)
                    Announce(menuName);

                ShowOptions(options);

                int command = Command.ParseIntCommand();
                if (command > 0 && command < options.Length)
                {
                    try
                    {
                        actions[command - 1]();
                    }
                    catch (Exception ex) when (ex is NullReferenceException || ex is NotImplementedException) // Should there be more options than actions available
                    {
                        Console.WriteLine("Option is Not Yet Implemented (NYI)\n");
                    }
                }
                else if (command - 1 == actions.Length) // Last option is always "exit" or "back"
                {
                    if (this is MainMenu) // Last option in these menus is "exit"
                        Game.IsRunning = false;
                    else if (this is PlayerMenu) // Reset player on logout
                    {
                        Game.CurrentPlayer?.Save();
                        Game.CurrentPlayer = null;
                        Game.CurrentCharacter = null;
                        Game.CurrentSkill = null;
                        Game.CurrentItem = null;
                        Console.Clear();
                    }
                        
                    return;
                }
                else
                    Console.WriteLine("Invalid Command.\n");
            }
        }
    }
}
