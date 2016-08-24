namespace Battles
{
    sealed class Eld : Buff
    {
        // Used by Eld Rune
        private const string name = "Eld";

        public Eld(int duration, float haste)
            : base(name, haste: haste, duration: duration, maxStacks: 1)
        {
        }

        protected override string Description() => $"Increases haste by {(int)Haste}; Turns left: {CurrentDuration}";
    }
}
