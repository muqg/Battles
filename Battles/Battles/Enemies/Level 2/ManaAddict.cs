namespace Battles.Enemies
{
    sealed class ManaAddict : Enemy
    {
        private const string name = "Mana Addict";
        private const int health = 300;
        private const int attack = 10;
        private const int level = 2;

        public ManaAddict() 
            : base(name, health, attack, level)
        {
        }

        protected override bool SkillHitEffect(CharacterStats player, Stats self, EffectValues skillEffectValues)
        {
            int attack = skillEffectValues.SourceSkill.Cost / 2;

            EnergySurge surge = new EnergySurge(attack);
            Buff.AddBuff(surge, self);
            surge.SetStacks();

            Menu.Announce($"Using your skill gives {Name} {surge.Name} gaining {attack} attack for its next turn");

            return true;
        }
    }
}
