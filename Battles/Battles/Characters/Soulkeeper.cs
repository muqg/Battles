using System;

namespace Battles.Characters
{
    [Serializable]
    sealed class Soulkeeper : Character
    {
        public Soulkeeper(string name)
            : base(name, "Soulkeeper")
        {
            Skills.Add(new Reflection());
            Skills.Add(new ChaosStrike());
            Skills.Add(new DrainSoul());
            Skills.Add(new DrawSoul());
        }
    }
}
