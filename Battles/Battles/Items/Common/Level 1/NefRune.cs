using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class NefRune : Item
    {
        private const string name = "Nef Rune";

        public NefRune()
            : base(name, ItemType.Consumable, level: 1, dropChance: 37)
        {
            // TODO:
        }
    }
}