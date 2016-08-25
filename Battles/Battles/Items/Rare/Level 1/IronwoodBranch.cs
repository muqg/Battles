using System;

namespace Battles.Items.Rare
{
    [Serializable]
    sealed class IronwoodBranch : Item
    {
        private const string name = "Ironwood Branch";
        private const int attributeBonus = 2;
        private const int dropChance = 27;

        public IronwoodBranch()
            : base(name,
                  rarity: ItemRarity.Rare,
                  level: 1,
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
