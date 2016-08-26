using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    class CharacterStats : Stats
    {
        public bool HasUsedItem = false;

        private int _maxMana;
        private int _mana;
        private int _spellPower;

        public CharacterStats(Character ownerUnit)
            : base(ownerUnit)
        {
            OwnerUnit = ownerUnit;
            MaxMana = OwnerUnit.Mana; // Setter of MaxMana also sets Mana since mana is increasing from 0 to a value
            ManaRegen = OwnerUnit.ManaRegeneration;
            SpellPower = OwnerUnit.SpellPower;

            ActiveItems = OwnerUnit.Items
                .Where(i => i.Type == Item.ItemType.Active)
                .Concat(OwnerUnit.Inventory
                    .Where(i => i.Type == Item.ItemType.Consumable && OwnerUnit.Level >= i.Level))
                .OrderBy(i => i.Type == Item.ItemType.Active)
                .ToList();
        }

        public int ManaRegen { get; set; }
        public new Character OwnerUnit { get; }
        public List<Item> LootItems { get; set; }
        public List<Item> ActiveItems { get; }

        public int MaxMana
        {
            get
            {
                return _maxMana;
            }
            set
            {
                int difference = value - _maxMana;
                _maxMana = value;
                if (difference > 0) // Add the increase of the maximum mana to the current Mana
                    Mana += difference;
            }
        }
        public int Mana
        {
            get
            {
                return _mana;
            }
            set
            {
                _mana = value.Clamp(0, MaxMana);
            }
        }

        public int SpellPower
        {
            get
            {
                return _spellPower;
            }
            set
            {
                _spellPower = Math.Max(0, value);
            }
        }

        public override string ToString()
        {
            Menu.Announce(OwnerUnit.Name + "'s stats");
            return string.Join("\n",
                $"Health: {Health}/{MaxHealth}",
                $"Mana: {Mana}/{MaxMana}",
                $"Attack: {Attack}",
                $"Armour: {Armour}",
                $"Spell Power: {SpellPower}",
                $"Haste: {Haste}",
                $"Health Regeneration: {HealthRegen}",
                $"Mana Regeneration: {ManaRegen}",
                "");
        }
    }
}
