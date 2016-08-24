using System;

namespace Battles.Items.Common
{
    [Serializable]
    sealed class Dirk : Item
    {
        private const string name = "Dirk";

        public Dirk() 
            : base(name, attack: 3, dropChance: 70)
        {
        }
    }
}
