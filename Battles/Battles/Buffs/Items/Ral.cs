namespace Battles
{
    sealed class Ral : Buff
    {
        // Used by Ral Rune

        private const string name = "Ral";
        private int minBurn;
        private int maxBurn;

        public Ral(int minBurn, int maxBurn)
            : base(name, maxStacks: Constants.RuneBuffStacks, duration: 3)
        {
            this.minBurn = minBurn;
            this.maxBurn = maxBurn;
        }

        protected override string Description() => $"Using a skill burns your enemy for {minBurn * Stacks}-{maxBurn * Stacks} damage.";

        public override bool OnSkillUse(CharacterStats player, Stats enemy, EffectValues skillValues)
        {
            int burn = random.Next(minBurn * Stacks, maxBurn * Stacks + 1);
            enemy.Health -= burn;
            Menu.Announce($"Using your skill burns {enemy.OwnerUnit.Name}");
            Battle.WriteEnemyHealth(Name, burn, enemy);

            return true;
        }
    }
}
