using System;

namespace Battles.Items.Rare
{
    [Serializable]
    sealed class CircletOfNobility : Item
    {
        private const string name = "Circlet of Nobility";
        private const int attributeBonus = 5;
        private const int dropChance = 35;

        public CircletOfNobility()
            : base(name,
                  rarity: ItemRarity.Rare, 
                  level: 2,
                  health: attributeBonus, 
                  attack: attributeBonus, 
                  mana: attributeBonus, 
                  spellPower: attributeBonus, 
                  haste: attributeBonus, 
                  dropChance: dropChance)
        {
        }
    }
}
