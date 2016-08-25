namespace Battles.MechanicalYeti
{
    sealed class WhirlingBlades : Buff
    {
        private const string name = "Whirling Blades";

        public WhirlingBlades()
            : base(name, attack: 10, duration: 2, maxStacks: 1, dispellable: false)
        {
        }

        protected override string Description() => $"Increases Attack by {Attack}; Turns left: {CurrentDuration}";
    }
}
