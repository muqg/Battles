namespace Battles.MechanicalYeti
{
    sealed class ArmourPlating : Buff
    {
        private const string name = "Armour Plating";

        public ArmourPlating()
            : base(name, armour: 10, duration: 2, maxStacks: 1, dispellable: false)
        {
        }

        protected override string Description() => $"Increases Armour by {Armour}; Turns left: {CurrentDuration}";
    }
}
