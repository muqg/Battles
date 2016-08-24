namespace Battles.Enemies
{
    sealed class FaerieDragon : Enemy
    {
        private const int phaseChance = 10;

        private int pseudoPhaseChance = phaseChance;

        public FaerieDragon() 
            : base("Faerie Dragon", 200, 30, 2)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            pseudoPhaseChance = phaseChance;
            base.Initialize(player, enemy);
        }

        public override bool OnAttackHit(Stats self, Stats attacker, EffectValues attackValues)
        {
            if(phaseShift())
            {
                Menu.Announce($"{Name} phases out and avoids your attack");
                return false;
            }

            base.OnAttackHit(self, attacker, attackValues);
            return true;
        }

        protected override bool SkillHitEffect(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            if(phaseShift())
            {
                Menu.Announce($"{Name} phases out avoiding your skill completely");
                return false;
            }

            return true;
        }

        private bool phaseShift()
        {
            if (Utility.GetPseudoChance(phaseChance, ref pseudoPhaseChance))
                return true;

            return false;
        }
    }
}
