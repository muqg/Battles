using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    [Serializable]
    abstract class Character : Unit, ISingleActionMenu
    {
        private const int nameLength = 4;
        private const string charactersNamespace = "Battles.Characters";

        private static CharacterMenu characterMenu = new CharacterMenu();

        private int _totalVictories = 0;

        public Character(string name, string characterClass)
            : base(name, Constants.DefaultHealth, Constants.DefaultAttack, Constants.DefaultHaste)
        {
            Class = characterClass.SplitCapital();
        }

        #region Delegates

        public static Action<Character> ViewMenuAction = delegate (Character ch)
        {
            Game.CurrentCharacter = ch;
            Menu.Announce(ch.Name);
            characterMenu.Show();
            Game.CurrentCharacter = null;
        };

        public static Action<Character> DeleteMenuAction = delegate (Character ch)
        {
            ch.delete();
        };

        public static Action<Character> SelectMenuAction = delegate (Character ch)
        {
            Game.CurrentCharacter = ch;
            Enemy enemy = Enemy.Select();
            if (enemy != null)
                Battle.Start(ch.Stats(), enemy.Stats()); // Start the actual battle
            Game.CurrentCharacter = null;
        };

        #endregion Delegates

        #region Properties

        public static string NoCharactersError { get; } = "You have no characters, create one instead.";
        public static Type[] CharacterClasses { get; } = Utility.GetTypes(charactersNamespace);

        public int Mana { get; protected set; } = Constants.DefaultMana;
        public int ManaRegeneration { get; protected set; } = Constants.DefaultManaRegeneration;
        public int SpellPower { get; protected set; } = Constants.DefaultSpellpower;
        public List<Skill> Skills { get; } = new List<Skill>();
        public List<Item> Items { get; } = new List<Item>(Constants.MaxEquippedItems);
        public List<Item> Inventory { get; } = new List<Item>(Constants.InventoryLimit);
        public string Class { get; }
        public int TotalVictories
        {
            get
            {
                return _totalVictories;
            }
            set
            {
                _totalVictories = _totalVictories + 1; // Only increment by one.
            }
        }

        protected int Experience { get; set; } = 0;
        protected int SkillPoints { get; set; } = 1;
        protected int ExperienceForLevel => (int)((Level + (Level * Level / 10f)) * Constants.LevelExperienceBase);

        #endregion Properties

        public static void Create()
        {
            List<Character> characters = Game.CurrentPlayer.Characters;
            if (characters.Count < Constants.MaxCharacters)
            {
                Menu.Announce("Create new character");
                while (true)
                {
                    Console.WriteLine($"Choose a name for your character{Constants.BackMessage}:");
                    string name = Command.ParseAlphaCommand();

                    if (name.Equals(Constants.BackCommand))
                        return;
                    else if (name.Length < nameLength)
                    {
                        Console.WriteLine("Name must be at least 4 characters long and must contain only letters.\n");
                        continue;
                    }
                    else if(characters.Find(c => c.Name.Equals(name)) != null)
                    {
                        Console.WriteLine("Character with that name already exists.\n");
                        continue;
                    }

                    while (true)
                    {
                        Console.WriteLine("Choose a class:");

                        // Get all classes
                        string[] classNames = CharacterClasses.Select(c => c.Name).ToArray();

                        Menu.ShowOptions(classNames.Select(c => c.SplitCapital()).ToArray()); // Show options to choose from

                        int command = Command.ParseIntCommand();
                        if(command > 0 && command <= classNames.Length)
                        {
                            Type characterClass = Utility.GetType((classNames[command - 1]).ToString());
                            characters.Add(Activator.CreateInstance(characterClass, name) as Character);

                            Console.WriteLine($"Character {name} created.\n");
                            Game.CurrentPlayer.Save();

                            return;
                        }
                        else
                        {
                            Console.WriteLine(Constants.InvalidCommand);
                            continue;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"You have the maximum of {Constants.MaxCharacters} characters created.\n");
            }
        }

        // Loads a character from already existing one.
        // Should prevent and handle deserialization errors mirroring any changes to constants and adding new parts by creating a fresh object
        public static Character Load(Character loadingCharacter)
        {
            Character character = Activator.CreateInstance(
                loadingCharacter.GetType(), 
                loadingCharacter.Name) as Character;

            character.Level = loadingCharacter.Level;
            character.Experience = loadingCharacter.Experience;
            character.SkillPoints = loadingCharacter.SkillPoints;

            for (int i = 0; i < loadingCharacter.Skills.Count; i++)
            {
                character.Skills[i] = Skill.Load(loadingCharacter.Skills[i]);
            }

            for(int i = 0; i < loadingCharacter.Inventory.Count; i++)
            {
                character.Inventory.Add(Item.Load(loadingCharacter.Inventory[i]));
            }

            for(int i = 0; i < loadingCharacter.Items.Count; i++)
            {
                character.Items.Add(Item.Load(loadingCharacter.Items[i]));
            }

            return character;
        }

        public override void Act(CharacterStats player, Stats enemy)
        {
            bool playerTurn = true;
            do
            {
                playerTurn = BattleMenu.Show(player, enemy);
            }
            while (playerTurn);

            base.Act(player, enemy);
        }

        public string Description()
        {
            CharacterStats stats = Stats();
            return string.Join("\n",
            $"Name: {Name}",
            $"Class: {Class}",
            $"Level: {Level}",
            $"Experience: {Experience}/{ExperienceForLevel}",
            $"Skill points: {SkillPoints}",
            "",
            $"Health: {stats.Health}",
            $"Mana: {stats.Mana}",
            $"Attack: {stats.Attack}",
            $"Armour: {stats.Armour}",
            $"SpellPower: {stats.SpellPower}",
            $"Haste: {stats.CurrentHaste}",
            $"Health Regeneration: {stats.HealthRegen}",
            $"Mana Regeneration: {stats.ManaRegen}",
            "",
            $"Total victories: {TotalVictories}",
            "");
        }

        // Used for preparations at the start of the battle
        public override void Initialize(CharacterStats player, Stats enemy)
        {
            // Refresh skills
            foreach (Skill s in Skills)
                s.Refresh();

            // Trigger initial item effects
            foreach (Item item in Items)
                item.OnBattleStart(player, enemy);

            base.Initialize(player, enemy);
        }

        public override bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues skillEffectValues) // Always call when overriding
        {
            foreach (Buff buff in player.Buffs)
                if (!buff.OnSkillHit(player, enemy, skillEffectValues)) return false; // Skill interrupted by buff effect

            foreach (Item item in player.OwnerUnit.Items)
                if (!item.OnSkillHit(player, enemy, skillEffectValues)) return false; // Skill interrupted by item effect

            return base.OnSkillHit(player, enemy, skillEffectValues);
        }

        public override string ToString() => $"{Name}, level {Level}, {Class}";

        // Equips an item (adds it to Items) -> Returns true on success; False on already fully equipped
        public bool EquipItem(Item item)
        {
            if(Items.Count >= Constants.MaxEquippedItems)
            {
                Console.WriteLine($"Can't equip more than {Constants.MaxEquippedItems} items.\n");
                return false;
            }
            if(item.Level > Level)
            {
                Console.WriteLine($"Item requires character level {item.Level}.\n");
                return false;
            }

            Inventory.Remove(item);
            Items.Add(item);
            Console.WriteLine($"Equipped {item.Name}.\n");

            // Save on equipped item
            Game.CurrentPlayer.Save();

            return true;
        }

        // Character experience gain. Levels the character up, adds extra experience on top and adds skill points for leveling.
        public void GainExperience(int experienceGained)
        {
            if (Level < Constants.MaxLevel)
            {
                Console.WriteLine($"You have gained {experienceGained} experience.\n");
                Experience += experienceGained;
                bool leveledOnce = Experience >= ExperienceForLevel; // Control variable for announcement
                while (Experience >= ExperienceForLevel)
                {
                    Experience -= ExperienceForLevel; // First manage experience since it is based on level
                    Level += 1;
                    SkillPoints += 1;
                }

                if (leveledOnce) // Announce level change if character has gained at least one level
                    Console.WriteLine($"Character {Name} has reached level {Level}.\n");
            }
        }

        // Levels a skill up, checking if it is ultimate (requiring level 6) and if the player has necessary skill points
        public void LevelSkill(Skill skill)
        {
            if(SkillPoints > 0)
            {
                if(Skills.Find(s => ReferenceEquals(s, skill)) != null)
                {
                    if(skill.IsUltimate && Level < Constants.UltimateLevel)
                    {
                        Console.WriteLine($"Ultimate skills require at least level {Constants.UltimateLevel} to be leveled.\n");
                    }
                    else if(skill.LevelUp())
                    {
                        SkillPoints -= 1;
                        Console.WriteLine($"{skill.Name} leveled up. Character {Name} has {SkillPoints} skill points left.\n");
                        Game.CurrentPlayer.Save(); // Save progress
                    }
                }
            }
            else
            {
                Console.WriteLine("Not enough skill points. Level up to gain more.\n");
            }
        }

        // Loots an item adding it to the inventory if there is free space -> True on success; False on full inventory
        public bool LootItem(Item item)
        {
            if(Inventory.Count >= Constants.InventoryLimit)
            {
                Console.WriteLine("Inventory is full.\n");
                return false;
            }

            Inventory.Add(Activator.CreateInstance(item.GetType()) as Item);
            Game.CurrentPlayer.Save(); // Save on looted item
            Console.WriteLine($"You receive loot {item.Name}.\n");
            return true;
        }

        public CharacterStats Stats()
        {

            CharacterStats stats = new CharacterStats(this);
            foreach (Item item in Items)
            {
                item.Equip(stats);
            }
            return stats;
        }

        // Unequips an item
        public bool UnequipItem(Item item)
        {
            if(Inventory.Count >= Constants.InventoryLimit)
            {
                Console.WriteLine("Inventory is already full. Can't unequip item.\n");
                return false;
            }

            Items.Remove(item);
            Inventory.Add(item);
            Console.WriteLine($"Unequipped {item.Name}.\n");

            // Save on unequiping item
            Game.CurrentPlayer.Save();

            return true;
        }

        protected sealed override void EndTurn(CharacterStats player, Stats enemy)
        {
            player.CheckBuffs(); // Check for buff effects
            player.HasUsedItem = false; // Reset item usage
            player.Health += player.HealthRegen;
            player.Mana += player.ManaRegen;

            foreach (Skill skill in Skills) // Manage skill cooldowns
                skill.CurrentCooldown = Math.Max(0, skill.CurrentCooldown - 1);

            foreach (Item item in Items)
                item.CurrentCooldown = Math.Max(0, item.CurrentCooldown - 1);
        }

        // Deletes the character
        protected void delete()
        {
            if(Utility.ConfirmAction())
            {
                List<Character> characters = Game.CurrentPlayer.Characters;
                Character ch = characters.Find(c => ReferenceEquals(c, this));

                if (ch != null)
                {
                    characters.Remove(this);
                    Game.CurrentPlayer.Save();
                    Console.WriteLine($"Character {Name} has been deleted.\n");
                }
                else
                    Console.WriteLine("There was an error. Character could not be deleted.\n");
            }
        }
    }
}
