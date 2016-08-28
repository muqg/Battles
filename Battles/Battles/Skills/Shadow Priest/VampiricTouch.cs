using System;

namespace Battles
{
    [Serializable]
    sealed class VampiricTouch : Skill
    {
        private const int heal = 20;
        private const int manaRestore = 5;

        private static ShadowEssence essence;

        public VampiricTouch() 
            : base("Vampiric Touch", SkillType.Magic, 30, true)
        {
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy) => player.SpellPower / 4 + Level * 10;

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player);
            int damage = Power(player, enemy) * essence.Stacks;
            int healing = heal * essence.Stacks;

            SkillEffectValues = new EffectValues(damage, healing, this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            int manaRestore = VampiricTouch.manaRestore * essence.Stacks;
            player.Mana += manaRestore;
            Battle.WritePlayerMana(Name, manaRestore, player);

            essence.Remove();
            essence.WriteStacks();
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy)
        {
            int stacks = Buff.AddBuff<ShadowEssence>(player).Stacks;
            int damage = Power(player, enemy);

            return $"Restores {heal} health and {manaRestore} mana to you and deals ({damage}) damage to your enemy per stack of {essence.Name}."
                + $"Consumes all ({stacks}) of {essence.Name}.";
        }

        protected override string SpecificDescription() => $"Restores health and mana to you and deals damage "
            + $"to your enemy per each stack of {essence.Name}. Stacks are consumed in the process.";
    }
}
