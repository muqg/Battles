using System;

namespace Battles
{
    [Serializable]
    sealed class MindFlay : Skill
    {
        private const int EssenceChancePercentage = 25;

        private static ShadowEssence essence;
        private static int EssenceGainPseudoChance;

        public MindFlay()
            : base("Mind Flay", SkillType.Magic, 20)
        {
        }

        private int EssenceGainChance => 25 * Level;

        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            return base.BattleDescription(player, enemy) + $"Deals {Power(player.SpellPower)} damage and has a {EssenceGainChance}% chance to grant a {essence.Name}.";
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();
            EssenceGainPseudoChance = EssenceGainChance;

            base.Refresh();
        }

        protected override int Power(int powerStat) => (Level * 10 + powerStat) / 2;

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            int damage = Power(player.SpellPower);
            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            if (Utility.GetPseudoChance(EssenceGainChance, ref EssenceGainPseudoChance))
            {
                essence = Buff.AddBuff<ShadowEssence>(player.Buffs);

                essence.SetStacks(player); // Add one stack
                essence.WriteStacks();
            }
        }

        protected override string SpecificDescription() => $"Flays the mind of your enemy for ({Power(Game.CurrentCharacter.SpellPower)}) damage "
            + $"and has a ({EssenceGainChance}%) chance to grant you a Shadow Essence.";
    }
}
