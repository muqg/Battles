namespace Battles.MechanicalYeti
{
    sealed class SparePart : Buff
    {
        private const string name = "Spare Part";

        public SparePart()
            : base(name, health: 10, dispellable: false)
        {
        }

        protected override string Description() => $"Increases Max Health by {Health * Stacks}";
    }
}
