using System.Collections.Generic;

namespace Battles.Enemies
{
    sealed class DustDevil : Enemy
    {
        private const string name = "Dust Devil";
        private const int health = 120;
        private const int attack = 30;
        private const int windfuryChance = 15;

        private int pseudoWindfuryChance;

        public DustDevil() 
            : base(name, health, attack)
        {
        }

        public override void Initialize(CharacterStats player, Stats self)
        {
            pseudoWindfuryChance = windfuryChance;
            base.Initialize(player, self);
        }

        protected override void ActBehaviour(CharacterStats player, Stats self)
        {
            base.ActBehaviour(player, self);

            if (Utility.GetPseudoChance(windfuryChance, ref pseudoWindfuryChance))
            {
                Menu.Announce($"{Name} gains Windfury allowing for an additional quick attack");
                self.OwnerUnit.AttackEnemy(self, player);
            }
        }
    }
}
