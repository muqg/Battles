using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class NefRune : Item
    {
        private const string name = "Nef Rune";
        private const string buffName = "Nef";
        private const float buffHaste = -4f;
        private const int buffDuration = 3;

        public NefRune()
            : base(name, ItemType.Consumable, level: 1, dropChance: 37)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Decreases your enemy's haste by {Math.Abs(buffHaste)} for {buffDuration} turns. Stacks up to {Constants.RuneBuffStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff nef = new HasteBuff(buffName, buffHaste, buffDuration, Constants.RuneBuffStacks);
            nef = Buff.AddBuff(nef, enemy);
            nef.SetStacks();

            Menu.Announce($"Using {Name} reduces {enemy.OwnerUnit.Name}'s Haste by {Math.Abs(buffHaste)}");
            nef.WriteEnemyStacks(enemy.OwnerUnit.Name);
        }
    }
}