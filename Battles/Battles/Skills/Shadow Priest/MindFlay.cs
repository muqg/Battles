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

        public override void Refresh()
        {
            essence = new ShadowEssence();
            EssenceGainPseudoChance = EssenceGainChance;

            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy) => (Level * 10 + player.SpellPower) / 2;

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            if (Utility.GetPseudoChance(EssenceGainChance, ref EssenceGainPseudoChance))
            {
                essence = Buff.AddBuff<ShadowEssence>(player);

                essence.SetStacks(); // Add one stack
                essence.WriteStacks();
            }
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy) => 
            $"Deals {Power(player, enemy)} damage and has a {EssenceGainChance}% chance to grant a {essence.Name}.";

        protected override string SpecificDescription() => $"Flays the mind of your enemy dealing damage "
            + $"and has a ({EssenceGainChance}%) chance to grant you a {essence.Name}. \nLevels increase damage and {essence.Name} gain chance.";
    }
}
