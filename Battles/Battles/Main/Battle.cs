using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    static class Battle
    {
        private static readonly LootMenu lootMenu = new LootMenu();

        public static CharacterStats Player { get; private set; } = null;
        public static Stats Enemy { get; private set; } = null;

        public static void Start(CharacterStats player, Stats enemy)
        {
            Player = player;
            Enemy = enemy;

            player.OwnerUnit.Initialize(player, enemy);
            enemy.OwnerUnit.Initialize(player, enemy);

            foreach (Stats unit in determineTurns(player, enemy))
            {
                unit.OwnerUnit.Act(player, enemy); // Unit turn

                if (checkVictoryDefeat(player, enemy)) // Check for victory or defeat
                    break;
            }
        }

        #region Write
        // Used for enemy damage taken, health gained or health drained
        public static void WriteEnemyHealth(string sourceName, float amount, Stats enemy, bool heal = false, bool drain = false)
        {
            if (heal)
                Console.WriteLine($"{enemy.OwnerUnit.Name} heals himself for {amount}.".Indent());
            else if (drain)
                Console.WriteLine($"{sourceName} drains {amount} health from {enemy.OwnerUnit.Name}".Indent());
            else
                Console.WriteLine($"{sourceName} hits {enemy.OwnerUnit.Name} for {amount} damage.".Indent());
            Console.WriteLine($"{enemy.OwnerUnit.Name} has {enemy.Health} health left.".Indent());
            Console.WriteLine();
        }

        // Used for enemy attacks on player
        public static void WritePlayerDamage(string enemyName, string sourceName, float damage, float playerHealth)
        {
            Console.WriteLine($"{enemyName}'s {sourceName} hits you for {damage} damage.".Indent());
            Console.WriteLine($"You have {playerHealth} health left.".Indent());
            Console.WriteLine();
        }

        // Used for player healing
        public static void WritePlayerHealthGain(string sourceName, float healthRestored, CharacterStats player)
        {
            Console.WriteLine($"{sourceName} restores {healthRestored} health to you.".Indent());
            Console.WriteLine($"You have {player.Health} health left.".Indent());
            Console.WriteLine();
        }

        // Used for player mana regen or drain
        public static void WritePlayerMana(string sourceName, float mana, CharacterStats player, bool restore = true)
        {
            if (restore) // Restores mana
                Console.WriteLine($"{sourceName} restores {mana} mana to you.".Indent());
            else // Drains mana
                Console.WriteLine($"{sourceName} drains {mana} mana from you.".Indent());
            Console.WriteLine($"You have {player.Mana} mana left.".Indent());
            Console.WriteLine();
        }
        #endregion Write

        private static bool checkVictoryDefeat(CharacterStats player, Stats enemy)
        {
            if (player.Health == 0)
            {
                // Save progress on defeat
                Game.CurrentPlayer.Save();

                Menu.Announce($"{enemy.OwnerUnit.Name} has defeated you in battle!");
                PressAnyKey();
                return true;
            }
            else if (enemy.Health == 0)
            {
                Enemy enemyUnit = enemy.OwnerUnit as Enemy;

                Menu.Announce($"You have defeated {enemyUnit.Name} in battle!");
                player.OwnerUnit.GainExperience(enemyUnit.ExperienceAward); // Award experience

                // Save progress on victory (after all gains except loot -> loot saves every time after looting or dropping an item)
                Game.CurrentPlayer.Save();

                player.LootItems = enemyUnit.Loot(); // Get loot for this battle
                if(player.LootItems?.Count > 0) // Check if there is any loot
                {
                    Console.WriteLine($"Defeating {enemyUnit.Name} has left you with some loot:");
                    lootMenu.Show();

                    Console.Clear();
                }
                else
                {
                    PressAnyKey();
                }

                return true;
            }

            return false;
        }

        private static IEnumerable<Stats> determineTurns(params Stats[] stats)
        {
            float leastHaste = 0;
            List<Stats> sortedStats = new List<Stats>();
            while (true) // Canceled in the calling foreach loop by the victory/defeat conditions
            {
                if (sortedStats.Count < 1) // Reset turns
                {
                    foreach (Stats s in stats)
                        s.CurrentHaste = s.Haste;

                    sortedStats = stats.OrderByDescending(s => s.CurrentHaste).ToList();
                }
                else
                {
                    // Always sort to mirror possible changes in units' haste
                    sortedStats = sortedStats.OrderByDescending(s => s.CurrentHaste).ToList();
                }

                // Last element has the least haste
                leastHaste = sortedStats[sortedStats.Count - 1].CurrentHaste;

                Stats first = sortedStats[0];
                first.CurrentHaste -= leastHaste;

                // Remove from list if element has less haste than the last one
                if (first.CurrentHaste < leastHaste)
                    sortedStats.Remove(first);

                yield return first;
            }

        }

        private static void PressAnyKey()
        {
            Console.WriteLine("\nPress any key to continue.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
