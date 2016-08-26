using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class EthRune : Item
    {
        private const string name = "Eth Rune";
        private const string buffName = "Eth";
        private const int buffArmour = -3;
        private const int buffMaxStacks = 5;
        private const int buffDuration = 3;

        public EthRune()
            : base(name, ItemType.Consumable, level: 1, dropChance: 37)
        {
        }

        protected override string ActiveEffectDescription { get; } = $"Reduces your enemy's armour by {Math.Abs(buffArmour)} for {buffDuration} turns. Stacks up to {buffMaxStacks} times.";

        protected override void ActiveEffect(CharacterStats player, Stats enemy)
        {
            Buff eth = new ArmourBuff(buffName, buffArmour, buffDuration, buffMaxStacks);
            eth = Buff.AddBuff(eth, enemy.Buffs) as Buff;
            eth.SetStacks(enemy);

            Menu.Announce($"Using {Name} decreases {enemy.OwnerUnit.Name}'s Armour by {Math.Abs(buffArmour)} for {buffDuration} turns.");
            eth.WriteEnemyStacks(enemy.OwnerUnit.Name);
        }
    }
}
