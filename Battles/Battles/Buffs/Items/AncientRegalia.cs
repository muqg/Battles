using System;

namespace Battles
{
    [Serializable]
    sealed class AncientRegalia : Buff
    {
        // Used by Bracer, Null Talisman, Wraith Band set
        private const string name = "Ancient Regalia";

        public AncientRegalia()
            : base(name, 
                  healthRegeneration: AncientRegaliaSet.Regen, 
                  manaRegeneration: AncientRegaliaSet.Regen, 
                  armour: AncientRegaliaSet.Armour, 
                  maxStacks: 1, 
                  dispellable: false)
        {
        }

        protected override string Description() => $"Increases Health and Mana regeneration by {HealthRegeneration} and Armour by {Armour}";
    }
}