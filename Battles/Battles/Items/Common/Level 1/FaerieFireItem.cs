using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class FaerieFireItem : Item
    {
        private const string name = "Faerie Fire";
        private const int healthRestore = 8;

        public FaerieFireItem()
            : base(name, ItemType.Active, level: 1, attack: 2, cooldown: 5, dropChance: 25)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Restores {healthRestore} health.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            player.Health += healthRestore;
            Battle.WritePlayerHealthGain(Name, healthRestore, player);
        }
    }
}
