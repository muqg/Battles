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

        private int StackGain => Level * 1;

        public override void Refresh()
        {
            essence = new ShadowEssence();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy) => player.SpellPower;

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player.Buffs);
            essence.SetStacks(player, StackGain);
            essence.WriteStacks();
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy) => $"Deals ({Power(player, enemy)}) damage and grants {StackGain} {essence.Name}.";

        protected override string SpecificDescription() => $"Shadows engulf you granting ({StackGain}) Shadow Essence and dealing "
            + $"damage to your enemy.\nLevels increase the amount of stacks gained.";
    }
}
