using System;

namespace Battles
{
    [Serializable]
    sealed class MindBlast : Skill
    {
        private static ShadowEssence essence;

        public MindBlast()
            : base("Mind Blast", SkillType.Magic, 8, cooldown: 3)
        {
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();

            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy) => (Level * 5) + (player.SpellPower / 4);

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player.Buffs);
            int damage = Power(player, enemy) * essence.Stacks;

            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            essence.Remove(player);
            essence.WriteStacks();
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy)
        {
            int stacks = Buff.AddBuff<ShadowEssence>(player.Buffs).Stacks;
            int damage = Power(player, enemy);

            return $"Consumes all({stacks}) of {essence.Name} to deal ({damage}) damage per stack.";
        }

        protected override string SpecificDescription() => $"Consumes all stacks of {essence.Name} to blast the mind of your enemy dealing damage per each stack.\nLevels increase damage.";
    }
}