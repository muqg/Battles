using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class RalRune : Item
    {
        private const string name = "Ral Rune";
        private const int minBurn = 1;
        private const int maxBurn = 5;

        public RalRune()
            : base(name, ItemType.Consumable, level: 2, dropChance: 34)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Causes your skills to burn your enemy for {minBurn}-{maxBurn} additional damage. Stacks up to {Constants.RuneBuffStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff ral = new Ral(minBurn, maxBurn);
            ral = Buff.AddBuff(ral, player);
            ral.SetStacks();

            Menu.Announce($"Using {Name} causes your skills to burn your enemy for additional damage");
            ral.WriteStacks();
        }
    }
}
