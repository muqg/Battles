using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    abstract class Enemy : Unit
    {
        private const string enemiesNamespace = "Battles.Enemies";

        public Enemy(string name, int health, int attack, int level = 1, EnemyType type = EnemyType.Normal, int armour = 0, float haste = Constants.DefaultHaste - 10)
            : base(name, health, attack, haste, level, armour)
        {
            Type = type;
            DropList = CreateDropList().Distinct().ToList(); // .Distinct() to avoid possible duplicate items
        }

        public enum EnemyType
        {
            Normal, Rare, Boss
        }

        public static List<List<Enemy>> EnemyList { get; } = initializeEnemyList(enemiesNamespace);

        public EnemyType Type { get; }
        public int ExperienceAward => random.Next(Level, (int)Math.Ceiling((double)Level + Level / 2) + 1);

        private List<Item> DropList { get; }

        public static Enemy Select()
        {
            while(true)
            {
                // Select level
                Menu.Announce("Select level");
                int levelCount = EnemyList.Count;
                
                // Get all level names in format
                string[] levels = new string[levelCount];
                for (int i = 0; i < levelCount; i++)
                {
                    levels[i] = $"Level {i + 1}";
                }

                // Show the levels to choose from as options
                Menu.ShowOptions(levels, true);

                int level = Command.ParseIntCommand() - 1;
                if (level >= 0 && level < levelCount)
                {
                    // Select enemy from chosen level
                    while(true)
                    {
                        int enemyCount = EnemyList[level].Count;
                        if (enemyCount == 0)
                            throw new NullReferenceException();

                        Menu.Announce("Select enemy");
                        Menu.ShowOptions(EnemyList[level].Select(e => e.Name).ToArray(), true);

                        int enemyNumber = Command.ParseIntCommand() - 1;
                        if (enemyNumber >= 0 && enemyNumber < enemyCount)
                            return EnemyList[level][enemyNumber]; // Return enemy
                        else if (enemyNumber == enemyCount)
                            break;

                        Console.WriteLine(Constants.InvalidCommand);
                    }
                }
                else if (level == levelCount)
                    return null;
                else
                    Console.WriteLine(Constants.InvalidCommand);
            }
        }

        // Enemy's final dropped loot 
        public virtual List<Item> Loot() => GetLootItems(DropList);

        public sealed override void Act(CharacterStats player, Stats self)
        {
            ActBehaviour(player, self);

            base.Act(player, self);
        }

        public sealed override bool OnSkillHit(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            foreach (Buff buff in self.Buffs)
            {
                if (!buff.OnSkillHit(player, self, skillEffectValues))
                    return false; // Skill usage interrupted by an effect
            }

            // Checks for base effects and then enemy's effect. If both return true then skill usage has not been interrupted and can proceed
            if (base.OnSkillHit(player, self, skillEffectValues) 
                && SkillHitEffect(player, self, skillEffectValues))
            {
                damageAndHealing(player, self, skillEffectValues);
                return true;
            }

            return false;
        }

        public Stats Stats() => new Stats(this);

        // Returns n amount of items from dropList at y% chance as a list of loot items
        protected static List<Item> GetLootItems(List<Item> dropList, int dropCount = 1, int dropChance = 80)
        {
            List<Item> droppedItems = dropList.Where(i => i.Drop()).ToList(); // Get all dropped items

            List<Item> loot = new List<Item>(dropCount); // Loot list
            for (int i = 0; i < dropCount && droppedItems.Count > 0; i++)
            {
                if (random.Next(0, 100) < dropChance) // Check an item is dropped by the unit
                {
                    int dropIndex = random.Next(0, droppedItems.Count);
                    loot.Add(droppedItems[dropIndex]);
                    droppedItems.Remove(droppedItems[dropIndex]);
                }
            }
            return loot;
        }

        // Behaviour for Act() method
        protected virtual void ActBehaviour(CharacterStats player, Stats self)
        {
            AttackEnemy(self, player);
        }

        // Creates the actual drop list
        protected virtual List<Item> CreateDropList()
        {
            List<Item> items = new List<Item>();
            items.AddRange(GetDropItems(Item.CommonItems));
            items.AddRange(GetDropItems(Item.RareItems));
            items.AddRange(GetDropItems(Item.SetItems, 2, 2));

            return items;
        }

        protected override void EndTurn(CharacterStats player, Stats self) // Always call when overriding
        {
            self.CheckBuffs(); // Check for buff effects
            self.Health += self.HealthRegen;
        }

        // The behaviour of the enemy for when hit by a skill
        // Returns true if skill has not been interrupted; False if skill has been interrupted and should not proceed
        protected virtual bool SkillHitEffect(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            return true;
        }

        // Gets a list of drop items from a list
        protected List<Item> GetDropItems(List<List<Item>> itemList, int lowerRange = 1, int upperRange = 1)
        {
            return itemList
                .Where(l => itemList.IndexOf(l) >= Level - lowerRange && itemList.IndexOf(l) <= Math.Min(Level + upperRange, Constants.MaxLevel))
                .SelectMany(i => i).ToList(); // Select them into a list of Items
        }

        // Handles the damage and healing part of the skill usend on the enemy
        private static void damageAndHealing(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            if (skillEffectValues.Damage >= 0)
            {
                self.Health -= skillEffectValues.Damage;
                Battle.WriteEnemyHealth(skillEffectValues.SourceSkill.Name, skillEffectValues.Damage, self);
            }
            if (skillEffectValues.Healing >= 0)
            {
                player.Health += skillEffectValues.Healing;
                Battle.WritePlayerHealthGain(skillEffectValues.SourceSkill.Name, skillEffectValues.Healing, player);
            }
        }

        // Initializes the list of enemies to their corresponding levels
        private static List<List<Enemy>> initializeEnemyList(string nameSpace)
        {
            // Create objects of all enemy types
            Enemy[] enemies = Utility.GetTypes(enemiesNamespace).Select(t => Activator.CreateInstance(t) as Enemy).ToArray();

            List<List<Enemy>> enemyList = new List<List<Enemy>>();
            // Add enemy objects to their corresponding level
            for (int i = 0; i < Constants.MaxLevel; i++)
            {
                enemyList.Add(new List<Enemy>()); // Initialize enemyList{i]
                enemyList[i].AddRange(enemies.Where(e => e.Level == i + 1).ToList()); // Add the list of enemies to the corresponding level
            }

            return enemyList;
        }
    }
}
