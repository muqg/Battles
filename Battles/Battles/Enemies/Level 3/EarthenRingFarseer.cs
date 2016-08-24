namespace Battles.Enemies
{
    sealed class EarthenRingFarseer : Enemy
    {
        private const string name = "Earthen Ring Farseer";
        private const int healChance = 17;
        private const int minHeal = 20;
        private const int maxHeal = 30;

        private int pseudoHealChance;

        public EarthenRingFarseer()
            : base(name, 300, 26, level: 3, armour: 3)
        {
        }

        public override void Initialize(CharacterStats player, Stats self)
        {
            pseudoHealChance = healChance;
            base.Initialize(player, self);
        }

        protected override void EndTurn(CharacterStats player, Stats self)
        {
            if(Utility.GetPseudoChance(healChance, ref pseudoHealChance))
            {
                int heal = random.Next(minHeal, maxHeal + 1);
                self.Health += heal;
                Menu.Announce($"{Name} heals himself using Earthen Blessing");
                Battle.WriteEnemyHealth("", heal, self, true);
            }

            base.EndTurn(player, self);
        }
    }
}
