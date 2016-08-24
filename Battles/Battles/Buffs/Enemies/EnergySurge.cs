namespace Battles
{
    sealed class EnergySurge : Buff
    { 
        // Used by Mana Addict
        private const string name = "Energy Surge";

        public EnergySurge(int attack)
            : base(name, duration: 1, attack: attack, maxStacks: 1)
        {
        }

        protected override string Description() => $"Increases Attack by {Attack}; Turns left: {Duration}";
    }
}