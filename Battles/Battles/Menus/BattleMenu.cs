using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    static class BattleMenu
    {
        private static readonly string[] options = { "Attack", "Skills", "Items", "Buffs", "Stats", "Concede" };

        // Returns false on completed turn. True if it is still player's turn
        public static bool Show(CharacterStats player, Stats enemy)
        {
            try
            {
                Menu.Announce($"{enemy.OwnerUnit.Name}: {enemy.Health}/{enemy.MaxHealth} health");
                Console.WriteLine($"Your health: {player.Health}/{player.MaxHealth}\n");
                Menu.ShowOptions(options);

                int command = Command.ParseIntCommand();
                switch (command)
                {
                    case 1:
                        player.OwnerUnit.AttackEnemy(player, enemy);
                        return false;
                    case 2:
                        Skill skill = chooseSkill(player, enemy);
                        if (skill != null)
                        {
                            if (skill.Use(player, enemy))
                                return false; // Successful usage
                            else
                                player.Mana += skill.Cost; // Replenish mana on failed usage
                        }

                        return true; // No skill has been used (e.g. back command)
                    case 3:
                        if (player.ActiveItems.Count > 0)
                        {

                            if (player.HasUsedItem)
                                Console.WriteLine(Constants.ItemUsedMessage);
                            else
                                chooseItem(player, enemy);
                        }
                        else
                            Console.WriteLine("There are no usable items.\n");

                        return true; // Items don't end the turn (not considered actions)
                    case 4:
                        enemy.ShowBuffs();
                        player.ShowBuffs();
                        return true; // This does not end player's turn
                    case 5:
                        Console.WriteLine(enemy);
                        Console.WriteLine(player);
                        return true; // This does not end player's turn
                    case 6:
                        Console.WriteLine("You concede\n");
                        player.Health = 0;
                        return false;
                    default:
                        Console.WriteLine(Constants.InvalidCommand);
                        return true;
                }
            }
            catch(NotImplementedException)
            {
                Console.WriteLine("Option is not yet implemented. (NYI)\n");
                return true;
            }
        }

        private static void chooseItem(CharacterStats player, Stats enemy)
        {
            while(true)
            {
                string[] options = player.ActiveItems.Select(i => i.BattleDescription()).ToArray();
                int itemCount = options.Length;
                Menu.Announce("Choose an item");

                Menu.ShowOptions(options, true);

                int command = Command.ParseIntCommand();
                if(command > itemCount + 1 || command <= 0)
                {
                    Console.WriteLine(Constants.InvalidCommand);
                    continue;
                }
                if (command > 0 && command <= itemCount)
                {
                    Item item = player.ActiveItems[command - 1];
                    if (item.CheckCooldown())
                    {
                        player.ActiveItems[command - 1].Use(player, enemy);
                        player.HasUsedItem = true;
                    }
                    else
                        continue; // Item is on cooldown
                }

                return; // Return on used item or back command
            }
        }

        private static Skill chooseSkill(CharacterStats player, Stats enemy)
        {
            while(true)
            {
                List<Skill> skills = player.OwnerUnit.Skills;
                string[] options = skills.Where(s => s.Level > 0).Select(s => s.BattleDescription(player, enemy)).ToArray();
                int skillCount = options.Length;
                Menu.Announce("Choose a skill");
                Console.WriteLine($"Mana: {player.Mana}/{player.MaxMana}\n");

                Menu.ShowOptions(options, true);

                int command = Command.ParseIntCommand();
                if (command > 0 && command <= skillCount)
                {
                    Skill skill = skills[command - 1];
                    if (skill.CheckCooldown() && skill.UseMana(player))
                        return skill;

                    continue;
                }
                else if (command == skillCount + 1)
                    return null;

                Console.WriteLine(Constants.InvalidCommand);
            }
        }
    }
}
