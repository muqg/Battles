using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class TirRune : Item
    {
        private const string name = "Tir Rune";
        private const int manaRestore = 10;

        public TirRune()
            : base(name, ItemType.Consumable)
        {
        }

        protected override string UsageDescription { get; } = $"Restores {manaRestore} mana.";

        protected override void ItemEffect(CharacterStats player, Stats enemy)
        {
            player.Mana += manaRestore;

            Battle.WritePlayerMana(Name, manaRestore, player);
        }
    }
}
