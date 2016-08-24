using System;

namespace Battles
{
    [Serializable]
    sealed class MindBlast : Skill
    {
        private static ShadowEssence essence;

        public MindBlast()
            : base("Mind Blast", SkillType.Magic, 8, cooldown: 2)
        {
        }

        // TODO: base.GetBattleInfo calls a virtual method instead
        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            int stacks = Buff.AddBuff<ShadowEssence>(player.Buffs).Stacks;

            return base.BattleDescription(player, enemy) + $"Consumes all({stacks}) {essence.Name} to deal {Power(player.SpellPower)} damage per stack.";
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();

            base.Refresh();
        }

        protected override int Power(int powerStat) => (Level * 5) + (powerStat / 4);

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player.Buffs);
            int damage = Power(player.SpellPower) * essence.Stacks;

            SkillEffectValues = new EffectValues(damage, source: this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            essence.Remove(player);
            essence.WriteStacks();
        }

        protected override string SpecificDescription() => $"Consumes all stacks of Shadow Essence to blast the mind of your enemy for ({Power(Game.CurrentCharacter.SpellPower)}) "
            + $"damage per stack. Has {Cooldown} turns cooldown.";
    }
}