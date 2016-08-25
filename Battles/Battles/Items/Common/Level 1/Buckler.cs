using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class Buckler : Item
    {
        private const string name = "Buckler";
        private const int blockChance = 50;
        private const int damageBlock = 2;

        private int pseudoBlockChance = blockChance;

        public Buckler()
            : base(name, dropChance: 50, level: 1)
        {
        }

        protected override string SpecialDescription { get; } = $"On being attacked has a {blockChance}% chance to block {damageBlock} damage";

        public override bool OnAttackHit(Stats self, Stats attacker, EffectValues effect)
        {
            if (base.OnAttackHit(self, attacker, effect))
            {
                if (Utility.GetPseudoChance(blockChance, ref pseudoBlockChance))
                {
                    Menu.Announce($"Your {Name} blocks {damageBlock} damage.");
                    effect.Damage -= damageBlock;
                }

                return true;
            }

            return false;
        }

        public override void OnBattleStart(CharacterStats player, Stats enemy)
        {
            base.OnBattleStart(player, enemy);
            pseudoBlockChance = blockChance;
        }
    }
}
