using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class ElRune : Item
    {
        private const string name = "El Rune";
        private const string buffName = "El";
        private const int buffArmour = 3;

        public ElRune()
            : base(name, ItemType.Consumable)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Increases your armour by {buffArmour} until the end of the battle.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff el = new ArmourBuff(buffName, buffArmour);
            el = Buff.AddBuff(el, player.Buffs) as Buff;
            el.SetStacks(player);

            Console.WriteLine($"Using {Name} increaeses your Armour by {buffArmour}.\n");
        }
    }
}
