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

        public override string BattleDescription(CharacterStats player, Stats enemy)
        {
            int stacks = Buff.AddBuff<ShadowEssence>(player.Buffs).Stacks;
            return base.BattleDescription(player, enemy) + $"Restores {heal} health and {manaRestore} mana to you and deals {Power(player.SpellPower)} damage to your enemy per stack of {essence.Name}. "
                + $"Consumes all({stacks}) {essence.Name}.";
        }

        public override void Refresh()
        {
            essence = new ShadowEssence();
            base.Refresh();
        }

        protected override int Power(int powerStat) => powerStat / 4 + Level * 10;

        protected override bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            essence = Buff.AddBuff<ShadowEssence>(player.Buffs);
            int damage = Power(player.SpellPower) * essence.Stacks;
            int healing = heal * essence.Stacks;

            SkillEffectValues = new EffectValues(damage, healing, this);

            return true;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            int manaRestore = VampiricTouch.manaRestore * essence.Stacks;
            player.Mana += manaRestore;
            Battle.WritePlayerMana(Name, manaRestore, player);

            essence.Remove(player);
            essence.WriteStacks();
        }

        protected override string SpecificDescription() => $"Restores {heal} health and {manaRestore} mana to you and deals ({Power(Game.CurrentCharacter.SpellPower)}) damage "
            + $"to your enemy per stack of {essence.Name}. Stacks are consumed.";
    }
}
