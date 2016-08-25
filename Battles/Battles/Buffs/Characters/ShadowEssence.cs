namespace Battles
{
    sealed class ShadowEssence : Buff
    {
        private const string name = "Shadow Essence";
        private const int power = 5;
        private const int stacks = 5;

        public ShadowEssence()
            : base(name, spellPower: power, maxStacks: stacks)
        {
        }

        protected override string Description() => $"Increases spellpower by {SpellPower * Stacks}";
    }
}
