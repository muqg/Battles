using System;

namespace Battles
{
    [Serializable]
    sealed class ShadowEmbrace : Skill
    {
        private static ShadowEssence essence;

        public ShadowEmbrace()
            : base("Shadow Embrace", SkillType.Magic, 35)
        {
        }

        private int StackGain => (int)Level * 1;

        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            return base.BattleDescription(player, enemy) + $"Deals {Power(player.SpellPower)} damage and grants {StackGain} {essence.Name}.";
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();
            base.Refresh();
        }

        protected override int Power(int powerStat) => powerStat;

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            int damage = Power(player.SpellPower);
            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player.Buffs);
            essence.SetStacks(player, StackGain);
            essence.WriteStacks();
        }

        protected override string SpecificDescription() => $"Shadows engulf you granting ({StackGain}) Shadow Essence and dealing "
            + $"{Power(Game.CurrentCharacter.SpellPower)} damage to your enemy.";
    }
}
