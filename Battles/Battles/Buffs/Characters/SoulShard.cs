namespace Battles
{
    sealed class SoulShard : Buff
    {
        private const string name = "Soul Shard";
        private const int regen = 1;

        public SoulShard()
            : base(name, healthRegeneration: regen)
        {
        }

        protected override string Description() => $"Increases Health regeneration by {regen * Stacks}";
    }
}
