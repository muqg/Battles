using System;

namespace Battles
{
    [Serializable]
    sealed class ChaosStrike : Skill
    {
        private const string name = "Chaos Strike";
        private const int cost = 4;

        private static SoulShard shard;

        public ChaosStrike()
            : base(name, SkillType.Attack, cost)
        {
        }

        private int attackModifier => 1 + Level / 4;
        private int shardModifier => Level;

        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            int power = Power(player, enemy);

            return base.BattleDescription(player, enemy) + $"Deals ({power}) damage to your enemy and ({power / 2}) damage to you.";
        }

        public override void Refresh()
        {
            shard = new SoulShard();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy)
        {
            shard = Buff.AddBuff<SoulShard>(player.Buffs);

            return player.Attack * attackModifier + shard.Stacks * shardModifier;
        }

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            int damage = Power(player, enemy);
            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            player.Health -= SkillEffectValues.Damage / 2;
            Battle.WritePlayerDamage(name, "effect", SkillEffectValues.Damage / 2, player.Health);
        }

        protected override string SpecificDescription() => $"A powerful strike that damages both you and your enemy. Deals ({Game.CurrentCharacter.Attack * attackModifier}) damage and "
            + $"an additional ({shardModifier}) damage for each {shard.Name}. Damages you for half that amount.";
    }
}
