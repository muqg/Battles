using System;

namespace Battles
{
    [Serializable]
    sealed class Reflection : Skill
    {
        private const string name = "Reflection";
        private const int cost = 15;
        private const int cooldown = 2;
        private const int soulShardCost = 1;

        private static SoulShard shard;


        public Reflection()
            : base(name, SkillType.Attack, cost, cooldown: cooldown)
        {
        }

        public override void Refresh()
        {
            shard = new SoulShard();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy) => (int)((player.Attack + enemy.Attack) * Level * .25f);

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            shard = Buff.AddBuff<SoulShard>(player);
            if (soulShardCost > shard.Stacks)
            {
                Console.WriteLine($"Not enough {shard.Name} to use skill.\n");
                return false;
            }

            int damage = Power(player, enemy);
            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            shard.SetStacks(-soulShardCost);
            shard.WriteStacks();
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy) =>
            $"Deals ({Power(player, enemy)}) damage to your enemy. (Requires {soulShardCost} {shard.Name})";

        protected override string SpecificDescription() => $"Conjures a shadow that strikes your enemy dealing damage, increased with your enemy's damage. "
            + $"(Requires {soulShardCost} {shard.Name})\nLevels increase damage.";
    }
}
