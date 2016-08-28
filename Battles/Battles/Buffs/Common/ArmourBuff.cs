using System;

namespace Battles
{
    sealed class ArmourBuff : Buff
    {
        // Used to increase or decrease armour of player or enemy

        public ArmourBuff(string name, int armour, int duration = -1, int maxStacks = 1)
            : base(name, duration: duration, armour: armour, maxStacks: maxStacks)
        {
        }

        protected override string Description() => $"{(Armour >= 0 ? "Increases" : "Decreases")} Armour by {Math.Abs(Armour) * Stacks}";
    }
}
