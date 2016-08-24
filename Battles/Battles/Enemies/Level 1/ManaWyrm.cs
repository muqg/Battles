namespace Battles.Enemies
{
    sealed class ManaWyrm : Enemy
    {
        private const string name = "Mana Wyrm";
        private const int health = 300;
        private const int attack = 10;
        private const int armour = 1;

        public ManaWyrm()
            : base(name, health, attack, armour: armour)
        {
        }

        protected override bool SkillHitEffect(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            if(base.SkillHitEffect(player, self, skillEffectValues))
            {
                int cost = skillEffectValues.SourceSkill.Cost;
                int heal = random.Next(cost / 3, (int)(cost / 1.5f));

                self.Health += heal;
                Menu.Announce($"{Name} feeds on the energy used for your skill");
                Battle.WriteEnemyHealth("", heal, self, true);
                return true;
            }

            return false;
        }
    }
}
