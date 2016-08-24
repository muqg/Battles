using System;

namespace Battles
{
    [Serializable]
    sealed class DrawSoul : Skill
    {
        private const string name = "Draw Soul";
        private const int cost = 0;
        private const int soulShardCost = 3;

        private static SoulShard shard;

        public DrawSoul() 
            : base(name, SkillType.Magic, cost, true)
        {
        }

        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            int power = Math.Abs(Power(player, enemy));

            return base.BattleDescription(player, enemy) + $"Deals ({power}) damage to your enemy and heals you for ({power}). (Requires {soulShardCost} {shard.Name})";
        }

        public override void Refresh()
        {
            shard = new SoulShard();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy)
        {
            int draw = (enemy.Health + player.Health) / 2;
            int power = Math.Min(draw - player.Health, player.MaxHealth - player.Health);

            return Math.Abs(power);
        }

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            shard = Buff.AddBuff<SoulShard>(player.Buffs);
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
            shard.SetStacks(player, -soulShardCost);
            shard.WriteStacks();
        }

        protected override string SpecificDescription() => "Attempts to draw your health to your enemy's. Heals you for the difference between your health and that of your enemy "
            + $"and deals damage to your enemy equal to the health you gained. (Requires {soulShardCost} {shard.Name})";
    }
}
