using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class MinorHealthPotion : Item
    {
        private const string name = "Minor Health Potion";
        private const int healthRestore = 8;

        public MinorHealthPotion()
            : base(name, ItemType.Consumable, dropChance: 70)
        {
        }

        protected override string UsageDescription { get; } = $"Restores {healthRestore} health.";

        protected override void ItemEffect(CharacterStats player, Stats enemy)
        {
            player.Health += healthRestore;
            Battle.WritePlayerHealthGain(Name, healthRestore, player);
        }
    }
}
