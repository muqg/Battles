using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class Buckler : Item
    {
        private const string name = "Buckler";
        private const int blockChance = 100;
        private const int damageBlock = 2;

        private int pseudoBlockChance = blockChance;

        public Buckler()
            : base(name, dropChance: 50, level: 1)
        {
        }

        public override void OnBattleStart(CharacterStats player, Stats enemy)
        {
            base.OnBattleStart(player, enemy);
            pseudoBlockChance = blockChance;
        }

        protected override bool AttackHitEffect(CharacterStats player, Stats enemy, EffectValues attackValues)
        {
            if (Utility.GetPseudoChance(blockChance, ref pseudoBlockChance))
            {
                Menu.Announce($"Your {Name} blocks {damageBlock} damage.");
                attackValues.Damage -= damageBlock;
            }

            return true;
        }

        protected override string PassiveEffectDescription { get; } = $"On being attacked has a {blockChance}% chance to block {damageBlock} damage";
    }
}
