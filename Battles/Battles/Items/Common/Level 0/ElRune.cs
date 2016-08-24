using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class ElRune : Item
    {
        private const string name = "El Rune";
        private const int armour = -2;
        private const int maxStacks = 5;
        private const int duration = 3;

        public ElRune()
            : base(name, ItemType.Consumable)
        {
        }

        protected override string UsageDescription { get; } = $"Reduces your enemy's armour by {Math.Abs(armour)} for {duration} turns. Stacks up to {maxStacks} times.";

        protected override void ItemEffect(CharacterStats player, Stats enemy)
        {
            El el = new El(duration, maxStacks, armour);
            Buff.AddBuff(el, enemy.Buffs);
            el.SetStacks(enemy);

            el.WriteEnemyStacks(enemy.OwnerUnit.Name);
        }
    }
}
