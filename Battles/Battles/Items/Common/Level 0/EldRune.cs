using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class EldRune : Item
    {
        private const string name = "Eld Rune";
        private const float haste = 20f;
        private const int duration = 3;

        public EldRune() 
            : base(name, ItemType.Consumable)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Increases your haste by {haste} for {duration} turns.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Eld eld = new Eld(duration, haste);
            eld = Buff.AddBuff(eld, player.Buffs) as Eld;
            eld.SetStacks(player);

            Console.WriteLine($"Using {Name} increases your Haste by {haste} for {duration} turns.\n");
        }
    }
}
