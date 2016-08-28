using System;

namespace Battles
{
    sealed class AttackBuff : Buff
    {
        // Used to increase or decrease Attack of player or enemy

        public AttackBuff(string name, int attack, int duration = -1, int maxStacks = 1)
            : base(name, attack: attack, duration: duration, maxStacks: maxStacks)
        {
        }

        protected override string Description() => $"{(Attack >= 0 ? "Increases" : "Decreases")} Attack by {Math.Abs(Attack) * Stacks}.";
    }
}
