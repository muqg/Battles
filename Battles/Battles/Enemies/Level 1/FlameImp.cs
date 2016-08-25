namespace Battles.Enemies
{
    sealed class FlameImp : Enemy
    {
        private const string name = "Flame Imp";
        private const int burstDamage = 30;
        private const int burstChance = 20;

        private static int pseudoBurstChance = burstChance;

        public FlameImp()
            : base(name, 230, 30)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            pseudoBurstChance = burstChance;
            base.Initialize(player, enemy);
        }

        protected override void ActBehaviour(CharacterStats player, Stats self)
        {
            base.ActBehaviour(player, self);

            flameBurst(player, self);
        }

        private void flameBurst(CharacterStats player, Stats enemy)
        {

            if (Utility.GetPseudoChance(burstChance, ref pseudoBurstChance))
            {
                player.Health -= burstDamage;
                enemy.Health -= burstDamage;

                Menu.Announce($"{Name} bursts in flames dealing {burstDamage} to both of you");
                Battle.WritePlayerDamage(enemy.OwnerUnit.Name, "Flame Burst", burstDamage, player.Health);
                Battle.WriteEnemyHealth("Flame Burst", burstDamage, enemy);
            }
        }
    }
}
