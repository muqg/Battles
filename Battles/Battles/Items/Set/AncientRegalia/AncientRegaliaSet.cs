using Battles.Items.Set;
using System;

namespace Battles
{
    [Serializable]
    sealed class AncientRegaliaSet : ItemSet
    {
        public const int Regen = 6;
        public const int Armour = 1;

        public AncientRegaliaSet() 
            : base(typeof(AncientRegalia), typeof(Bracer), typeof(WraithBand), typeof(NullTalisman))
        {
        }

        public override string Description { get; } = $"Set increases Health and Mana regeneration by {Regen} and Armour by {Armour}.";
    }
}
