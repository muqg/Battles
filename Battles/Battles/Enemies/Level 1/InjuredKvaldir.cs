using System;

namespace Battles.Enemies
{
    sealed class InjuredKvaldir : Enemy
    {
        private const string name = "Injured Kvaldir";
        private const int bleedChance = 30;
        private const int minBleed = 10;
        private const int maxBleed = 25;

        private static int pseudoBleedChance = bleedChance;
        
        public InjuredKvaldir()
            : base(name, 400, 20, armour: 2)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            pseudoBleedChance = bleedChance;
            base.Initialize(player, enemy);
        }

        protected override void EndTurn(CharacterStats player, Stats self)
        {
            if(Utility.GetPseudoChance(bleedChance, ref pseudoBleedChance))
            {
                int bleed = random.Next(minBleed, maxBleed);
                self.Health -= bleed;
                Menu.Announce($"{Name}'s injueries cause him to bleed out for {bleed} damage.");
                Battle.WriteEnemyHealth("Bleed", bleed, self);
            }

            base.EndTurn(player, self);
        }
    }
}
