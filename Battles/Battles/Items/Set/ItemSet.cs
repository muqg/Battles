using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    [Serializable]
    abstract class ItemSet
    {
        private const int regen = 6;
        private const int armour = 1;

        public ItemSet(Type setBuff, params Type[] setItems)
        {
            Items = setItems.ToList();
            Buff = setBuff;
        }

        public abstract string Description { get; }

        public Type Buff { get; }
        public List<Type> Items { get; }
    }
}
