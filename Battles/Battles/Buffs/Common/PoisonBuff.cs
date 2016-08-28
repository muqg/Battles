using System;

namespace Battles
{
    sealed class PoisonBuff : Buff
    {
        // Used to as a damage over time buff

        private int damagePerTick;

        public PoisonBuff(string name, int damagePerTick, int maxStacks = 1, int duration = 3)
            : base(name, maxStacks: maxStacks, duration: duration)
        {
            this.damagePerTick = Math.Max(0, damagePerTick);
        }

        protected override string Description() => $"Deals {damagePerTick * Stacks} damage per turn";

        protected override void Effect()
        {
            int damage = damagePerTick * Stacks;
            OwnerStats.Health -= damage;
            Battle.WriteEnemyHealth(Name, damage, OwnerStats);
        }
    }
}
