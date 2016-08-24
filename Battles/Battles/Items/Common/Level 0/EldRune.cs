using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class EldRune : Item
    {
        private const string name = "Eld Rune";
        private const float haste = 10f;
        private const int duration = 3;

        public EldRune() 
            : base(name, ItemType.Consumable)
        {
        }

        protected override string UsageDescription { get; } = $"Increases your haste by {haste} for {duration} turns.";

        protected override void ItemEffect(CharacterStats player, Stats enemy)
        {
            Eld eld = new Eld(duration, haste);
            Buff.AddBuff(eld, player.Buffs);
            eld.SetStacks(player);

            Console.WriteLine($"Using {Name} increases your Haste by {haste} for {duration} turns.");
        }
    }
}
