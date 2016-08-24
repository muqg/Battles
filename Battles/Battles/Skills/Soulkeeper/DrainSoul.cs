using System;

namespace Battles
{
    [Serializable]
    sealed class DrainSoul : Skill
    {
        private const string name = "Drain Soul";
        private const int cost = 40;

        private static SoulShard shard;

        public DrainSoul() 
            : base(name, SkillType.Magic, cost)
        {
        }


        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            return base.BattleDescription(player, enemy) + $"Drains ({Power(player.SpellPower)}) health from your enemy and generates a {shard.Name}.";
        }

        public override void Refresh()
        {
            shard = new SoulShard();
            base.Refresh();
        }

        protected override int Power(int powerStat) => powerStat / 4 + Level * 10;

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            int drain = Power(player.SpellPower);
            SkillEffectValues = new EffectValues(drain, drain, this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            shard = Buff.AddBuff<SoulShard>(player.Buffs);
            shard.SetStacks(player); // Add one stack
            shard.WriteStacks();
        }

        protected override string SpecificDescription() => $"Drains the soul of your enemy draining ({Power(Game.CurrentCharacter.SpellPower)}) health "
            + $"and generates a {shard.Name}.";
    }
}
