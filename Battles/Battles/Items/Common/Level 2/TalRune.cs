using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class TalRune : Item
    {
        private const string name = "Tal Rune";
        private const string buffName = "Tal";
        private const int buffDamage = 6;
        private const int buffDuration = 5;

        public TalRune()
            : base(name, ItemType.Consumable, level: 2, dropChance: 34)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Poisons your enemy dealing {buffDamage * buffDuration} damage over {buffDuration} turns. Stacks up to {Constants.RuneBuffStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff tal = new PoisonBuff(buffName, buffDamage, Constants.RuneBuffStacks, buffDuration);
            tal = Buff.AddBuff(tal, enemy);
            tal.SetStacks();

            Menu.Announce($"{Name} poisons {enemy.OwnerUnit.Name} over {buffDuration} turns.");
            tal.WriteEnemyStacks(enemy.OwnerUnit.Name);
        }
    }
}
