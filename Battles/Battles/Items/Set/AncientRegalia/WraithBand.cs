using System;

namespace Battles.Items.Set
{
    [Serializable]
    sealed class WraithBand : Item
    {
        private const string name = "Wraith Band";
        private const int bonus = 3;
        private const int dropChance = 30;

        public WraithBand()
            : base(name,
                  rarity: ItemRarity.Set,
                  level: 3,
                  health: bonus,
                  mana: bonus,
                  attack: bonus,
                  spellPower: bonus,
                  haste: bonus * 2,
                  armour: bonus,
                  dropChance: dropChance)
        {
        }

        protected override ItemSet Set { get; } = new AncientRegaliaSet();
    }
}
