using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    [Serializable]
    class Item : IExtendedBattleEvents, ISingleActionMenu
    {
        public const string ZeroItemsError = "You have no items.";

        private const string itemNamespace = "Battles.Items";

        protected static readonly Random random = new Random();

        #region Constructors
        public Item(string name,
            ItemType type = ItemType.Normal,
            ItemRarity rarity = ItemRarity.Common,
            int level = 0,
            int health = 0,
            int healthRegeneration = 0,
            int mana = 0,
            int manaRegeneration = 0,
            int attack = 0,
            int armour = 0,
            float haste = 0,
            int spellPower = 0,
            int cooldown = 0,
            int dropChance = 40)
        {
            Name = name;
            Level = level;
            Type = type;
            Rarity = rarity;

            Health = health;
            HealthRegeneration = healthRegeneration;
            Mana = mana;
            ManaRegeneration = manaRegeneration;
            Attack = attack;
            Armour = armour;
            Haste = haste;
            SpellPower = spellPower;
            Cooldown = cooldown;
            DropChance = dropChance;
        }
        #endregion Constructors

        public enum ItemRarity
        {
            Common, Rare, Epic, Legendary, Set
        }

        public enum ItemType
        {
            Normal, Usable, Consumable
        }

        #region Delegates
        public static Action<Item> DropAction = delegate (Item item)
        {
            if(Utility.ConfirmAction())
            {
                Game.CurrentCharacter.Inventory.Remove(item);
                Game.CurrentPlayer.Save(); // Save on dropped item
                Console.WriteLine($"You dropped {item.Name}\n");
            }
        };

        public static Action<Item> EquipAction = delegate (Item item)
        {
            Game.CurrentCharacter.EquipItem(item);
        };

        public static Action<Item> UneqipAction = delegate (Item item)
        {
            Game.CurrentCharacter.UnequipItem(item);
        };

        public static Action<Item> IntenvtoryAction = delegate (Item item)
        {
            Console.WriteLine(item.Description() + "\n");
        };

        public static Action<Item> LootAction = delegate (Item item)
        {
            if(Battle.Player.OwnerUnit.LootItem(item))
            {
                Battle.Player.LootItems.Remove(item);
            }
        };
        #endregion Delegates

        #region Properties
        public static List<List<Item>> CommonItems { get; } = initializeItemList(itemNamespace + ".Common");
        public static List<List<Item>> RareItems { get; } = initializeItemList(itemNamespace + ".Rare");
        public static List<List<Item>> SetItems { get; } = initializeItemList(itemNamespace + ".Set");

        public string Name { get; }
        public int Level { get; }
        public ItemRarity Rarity { get; }
        public ItemType Type { get; }
        public int CurrentCooldown { get; set; } = 0;

        protected virtual string SpecialDescription { get; } = "";
        protected virtual string UsageDescription { get; } = "";
        protected virtual ItemSet Set { get; } = null;

        protected int Health { get; }
        protected int HealthRegeneration { get; }
        protected int Mana { get; }
        protected int ManaRegeneration { get; }
        protected int Attack { get; }
        protected int Armour { get; }
        protected float Haste { get; }
        protected int SpellPower { get; }
        protected int Cooldown { get; }
        protected int DropChance { get; }
        #endregion Properties

        public static Item Load(Item loadingItem)
        {
            Item item = Activator.CreateInstance(loadingItem.GetType()) as Item;

            return item;
        }

        // Description used during the battle
        public string BattleDescription() => $"{Name} ({Type.ToString()}) - {UsageDescription}{(CurrentCooldown > 0 ? $"| Cooldown: {CurrentCooldown}" : "")}";

        // Checks whether the item is on cooldown
        public bool CheckCooldown()
        {
            if (CurrentCooldown <= 0)
                return true;

            string turns = CurrentCooldown == 1 ? "turn" : "turns";
            Console.WriteLine($"Item is on cooldown for {CurrentCooldown} more {turns}.\n");
            return false;
        }

        // General description of the item
        public string Description()
        {
            List<string> str = new List<string>();
            str.Add($"Name: {Name}");
            str.Add($"Level: {Level}");
            str.Add($"Rarity: {Rarity}");
            str.Add($"Type: {Type}");

            if(Health > 0)
                str.Add($"Health: {Health}");
            if (Mana > 0)
                str.Add($"Mana: {Mana}");
            if (Attack > 0)
                str.Add($"Attack: {Attack}");
            if (Armour > 0)
                str.Add($"Armour: {Armour}");
            if (SpellPower > 0)
                str.Add($"Spell Power: {SpellPower}");
            if (Haste > 0)
                str.Add($"Haste: {Haste}");
            if (HealthRegeneration > 0)
                str.Add($"Health Regeneration: {HealthRegeneration}");
            if (ManaRegeneration > 0)
                str.Add($"Mana Regeneration: {ManaRegeneration}");

            string setDescr = getSetDescription(Set);
            if (setDescr.Length > 0)
                str.Add("\n" + setDescr);

            if (SpecialDescription.Length > 0)
                str.Add("\n" + SpecialDescription);

            if (UsageDescription.Length > 0)
                str.Add("\nUse: " + UsageDescription);

            return string.Join("\n", str);
        }

        // Checks if item should drop in the loot
        public bool Drop()
        {
            if (random.Next(0, 100) < DropChance)
                return true;
            return false;
        }

        // Used when building the CharacterStats
        public void Equip(CharacterStats player)
        {
            checkSet(player, Set);

            player.MaxHealth += Health;
            player.HealthRegen += HealthRegeneration;
            player.MaxMana += Mana;
            player.ManaRegen += ManaRegeneration;
            player.Attack += Attack;
            player.Armour += Armour;
            player.CurrentHaste = player.Haste += Haste; // Set both MaxHaste and Haste here
            player.SpellPower += SpellPower;
        }

        // Uses the item
        public void Use(CharacterStats player, Stats enemy)
        {
            ItemEffect(player, enemy); // Actual effect of the item

            if (Type == ItemType.Consumable) // Remove the item if it is consumable
            {
                player.OwnerUnit.Inventory.Remove(this);
                player.ActiveItems.Remove(this);
                Game.CurrentPlayer.Save(); // Save on consuming an item
            }
            else if(Cooldown > 0) // Set cooldown if any
                CurrentCooldown = Cooldown + 1; // +1 since one turn is immediately skipped at the end of this one
        }

        public virtual bool OnAttackHit(Stats self, Stats attacker, EffectValues effect)
        {
            return true;
        }

        public virtual bool OnAttackUse(CharacterStats player, Stats enemy)
        {
            return true;
        }

        // Called at the start of the battle; Always call base when overriding
        public virtual void OnBattleStart(CharacterStats player, Stats enemy) // Always call when overriding
        {
            checkSet(player, Set);
            CurrentCooldown = 0;
        }

        public virtual bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues effect)
        {
            return true;
        }

        public virtual bool OnSkillUse(CharacterStats player, Stats enemy)
        {
            return true;
        }

        public override string ToString() => $"{Name}, level {Level} ({Type.ToString()})";

        // Item effect for usable and consumable items for when used
        protected virtual void ItemEffect(CharacterStats player, Stats enemy) { }

        // Checks if all set items are equipped and adds the corresponding set buff
        private static void checkSet(CharacterStats player, ItemSet set)
        {
            if(set != null && !player.Buffs.Any(b => b.GetType() == set.Buff))
            {
                foreach (Type t in set.Items)
                {
                    if (!Game.CurrentCharacter.Items.Any(i => i.GetType() == t))
                        return; // Set is not full
                }

                // Full set is present
                Buff buff = Activator.CreateInstance(set.Buff) as Buff;
                Buff.AddBuff(buff, player.Buffs);
                buff.SetStacks(player);
            }
        }

        // Initializes items of type (common, rare, etc.) into a list with corresponding levels
        private static List<List<Item>> initializeItemList(string nameSpace)
        {
            // Create objects of all item types in namespace
            Item[] enemies = Utility.GetTypes(nameSpace).Select(t => Activator.CreateInstance(t) as Item).ToArray();

            List<List<Item>> itemList = new List<List<Item>>();
            // Add item objects to their corresponding level
            for (int i = 0; i <= Constants.MaxLevel; i++)
            {
                itemList.Add(new List<Item>()); // Initialize itemList[i]
                itemList[i].AddRange(enemies.Where(e => e.Level == i).ToList()); // Add the item list to the corresponding level
            }

            return itemList;
        }

        // Provides description for equipped and not equipped items of a set. REQUIRES  Game.CurrentCharacter to be set!!
        private static string getSetDescription(ItemSet set)
        {
            if (set != null)
            {
                List<string> str = new List<string>();
                str.Add("Set:");
                foreach (Type t in set.Items)
                {
                    if (Game.CurrentCharacter.Items.Any(i => i.GetType() == t))
                        str.Add("1/1 " + t.Name.SplitCapital());
                    else
                        str.Add("0/1 " + t.Name.SplitCapital());
                }
                str.Add(set.Description);

                return string.Join("\n", str);
            }

            return "";
        }
    }
}
