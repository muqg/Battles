namespace Battles.Enemies
{
    sealed class KnifeJuggler : Enemy
    {
        private const int throwChance = 16;
        private const int minThrowDamage = 7;
        private const int maxThrowDamage = 15;

        private int throwPseudoChance = throwChance;

        public KnifeJuggler() 
            : base("Knife Juggler", 200, 20, 2)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            throwPseudoChance = throwChance;
            base.Initialize(player, enemy);
        }

        public override bool OnAttackHit(Stats self, Stats attacker, EffectValues effect)
        {
            throwKnife(attacker as CharacterStats, self);

            return base.OnAttackHit(self, attacker, effect);
        }

        protected override void ActBehaviour(CharacterStats player, Stats self)
        {
            throwKnife(player, self);
            AttackEnemy(self, player);
        }

        protected override bool SkillHitEffect(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            throwKnife(player, self);

            return true;
        }

        private void throwKnife(CharacterStats player, Stats self)
        {
            if (Utility.GetPseudoChance(throwChance, ref throwPseudoChance))
            {
                int throwDamage = random.Next(minThrowDamage, maxThrowDamage + 1);
                player.Health -= throwDamage;

                Menu.Announce($"{Name} hurls a knife at you dealing {throwDamage} damage.");
                Battle.WritePlayerDamage(Name, "knife", throwDamage, player.Health);
            }
        }
    }
}
