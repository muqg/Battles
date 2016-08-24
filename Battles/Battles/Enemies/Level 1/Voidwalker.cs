namespace Battles.Enemies
{
    class Voidwalker : Enemy
    {
        private const int drainChance = 15;

        private int pseudoDrainChance = drainChance;
        private int drainAmount = 15;

        public Voidwalker()
            : base("Voidwalker", 300, 11)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            pseudoDrainChance = drainChance;
            base.Initialize(player, enemy);
        }

        protected override void ActBehaviour(CharacterStats player, Stats self)
        {
            if (Utility.GetPseudoChance(drainChance, ref pseudoDrainChance))
            {
                player.Mana -= drainAmount;
                self.Health += drainAmount;

                Menu.Announce($"{Name} draws energy from you draining mana and gaining health.");
                Battle.WritePlayerMana(Name, drainAmount, player, false);
                Battle.WriteEnemyHealth(Name, drainAmount, self, true);
            }
            else
                AttackEnemy(self, player);
        }
    }
}
