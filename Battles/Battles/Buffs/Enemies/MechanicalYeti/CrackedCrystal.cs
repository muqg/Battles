namespace Battles.MechanicalYeti
{
    sealed class CrackedCrystal : Buff
    {
        private const string name = "Cracked Crystal";

        public CrackedCrystal()
            : base(name, spellPower: 15, duration: 3, maxStacks: 1, dispellable: false)
        {
        }

        protected override string Description() => $"Increases Spell Power by {SpellPower}";
    }
}
