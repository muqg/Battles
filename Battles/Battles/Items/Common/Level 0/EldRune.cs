using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class EldRune : Item
    {
        private const string name = "Eld Rune";
        private const string buffName = "Eld";
        private const float buffHaste = 8f;
        private const int buffDuration = 3;

        public EldRune() 
            : base(name, ItemType.Consumable)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Increases your haste by {buffHaste} for {buffDuration} turns. Stacks up to {Constants.RuneBuffStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff eld = new HasteBuff(buffName, buffHaste, buffDuration, Constants.RuneBuffStacks);
            eld = Buff.AddBuff(eld, player);
            eld.SetStacks();

            Menu.Announce($"Using {Name} increases your Haste by {buffHaste} for {buffDuration} turns.");
            eld.WriteStacks();
        }
    }
}
