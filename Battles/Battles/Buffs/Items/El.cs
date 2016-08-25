using System;

namespace Battles
{
    sealed class El : Buff
    {
        // Used by El Rune
        private const string name = "El";

        public El(int duration, int maxStacks, int armour)
            : base(name, duration: duration, armour: armour, maxStacks: maxStacks)
        {
        }

        protected override string Description() => $"Decreases armour by {Math.Abs(Armour) * Stacks}";
    }
}
