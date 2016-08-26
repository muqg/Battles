using System;

namespace Battles.Items.Rare
{
    [Serializable]
    sealed class PoorMansShield : Item
    {
        private const string name = "Poor Man's Shield";
        private const int blockChance = 65;
        private const int minBlock = 2;
        private const int maxBlock = 6;

        private int pseudoBlockChance = blockChance;

        public PoorMansShield()
            : base(name, rarity: ItemRarity.Rare, level: 3, haste: 6, armour: 1, dropChance: 25)
        {
        }

        public override void OnBattleStart(CharacterStats player, Stats enemy)
        {
            base.OnBattleStart(player, enemy);
            pseudoBlockChance = blockChance;
        }

        protected override bool AttackHitEffect(CharacterStats player, Stats enemy, EffectValues effect)
        {
            if (Utility.GetPseudoChance(blockChance, ref pseudoBlockChance))
            {
                int block = random.Next(minBlock, maxBlock + 1);
                effect.Damage -= block;
                Menu.Announce($"Your {Name} blocks {block} damage.");
            }

            return true;
        }

        protected override string PassiveEffectDescription { get; } = $"On being attacked has a {blockChance}% chance to block {minBlock}-{maxBlock} damage.";
    }
}
