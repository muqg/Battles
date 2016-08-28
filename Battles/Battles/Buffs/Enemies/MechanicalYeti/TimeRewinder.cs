namespace Battles.MechanicalYeti
{
    sealed class TimeRewinder : Buff
    {
        private const string name = "Time Rewinder";

        public TimeRewinder()
            : base(name, haste: 50, duration: 3, dispellable: false)
        {
        }

        protected override string Description() => $"Increases haste by {Stacks * Haste}";
    }
}
