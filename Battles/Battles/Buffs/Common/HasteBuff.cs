using System;

namespace Battles
{
    sealed class HasteBuff : Buff
    {
        // Used to increase or decrease Haste of player or enemy

        public HasteBuff(string name, float haste, int duration = -1, int maxStacks = 1)
            : base(name, haste: haste, duration: duration, maxStacks: maxStacks)
        {
        }

        protected override string Description() => $"{(Haste >= 0 ? "Increases" : "Decreases")} Haste by {Math.Abs(Haste) * Stacks}";
    }
}
