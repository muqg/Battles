using System;

namespace Battles.Items.Set
{
    [Serializable]
    sealed class Bracer : Item
    {
        private const string name = "Bracer";
        private const int bonus = 3;
        private const int dropChance = 30;

        public Bracer()
            : base(name,
                  rarity: ItemRarity.Set,
                  level: 3,
                  health: bonus * 2,
                  mana: bonus,
                  attack: bonus * 2,
                  spellPower: bonus,
                  haste: bonus,
                  armour: bonus / bonus,
                  dropChance: dropChance)
        {
        }

        protected override ItemSet Set { get; } = new AncientRegaliaSet();
    }
}
