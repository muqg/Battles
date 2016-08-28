using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class IthRune : Item
    {
        private const string name = "Ith Rune";
        private const string buffName = "Ith";
        private const int buffAttack = 6;
        private const int buffDuration = 4;

        public IthRune()
            : base(name, ItemType.Consumable, level: 1, dropChance: 37)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Increases attack by {buffAttack} for {buffDuration} turns. Stacks up to {Constants.RuneBuffStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff ith = new AttackBuff(buffName, buffAttack, buffDuration, Constants.RuneBuffStacks);
            ith = Buff.AddBuff(ith, player);
            ith.SetStacks();

            Menu.Announce($"Using {Name} increases your attack by {buffAttack}");
            ith.WriteStacks();
        }
    }
}
