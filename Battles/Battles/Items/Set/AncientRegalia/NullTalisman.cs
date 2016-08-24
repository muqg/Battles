using System;

namespace Battles.Items.Set
{
    [Serializable]
    sealed class NullTalisman : Item
    {
        private const string name = "Null Talisman";
        private const int bonus = 3;
        private const int dropChance = 30;

        public NullTalisman()
            : base(name,
                  rarity: ItemRarity.Set,
                  level: 3,
                  health: bonus,
                  mana: bonus * 2,
                  attack: bonus,
                  spellPower: bonus * 2,
                  haste: bonus,
                  armour: bonus / bonus,
                  dropChance: dropChance)
        {
        }

        protected override ItemSet Set { get; } = new AncientRegaliaSet();
    }
}
