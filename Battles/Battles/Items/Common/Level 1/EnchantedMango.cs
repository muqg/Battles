using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class EnchantedMango : Item
    {
        private const string name = "Enchanted Mango";
        private const int manaRestore = 15;

        public EnchantedMango()
            : base(name, ItemType.Active, level: 1, healthRegeneration: 1, cooldown: 5, dropChance: 25)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Restores 15 mana.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            player.Mana += manaRestore;
            Battle.WritePlayerMana(Name, manaRestore, player);
        }
    }
}
