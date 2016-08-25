namespace Battles.MechanicalYeti
{
    sealed class TimeRewinder : Buff
    {
        private const string name = "Time Rewinder";

        public TimeRewinder()
            : base(name, haste: 50, duration: 2, dispellable: false)
        {
        }

        protected override string Description() => $"Increases haste by {Stacks * Haste}; Turns left: {CurrentDuration}; Stacks: {Stacks}";
    }
}
