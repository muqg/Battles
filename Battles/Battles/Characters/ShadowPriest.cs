using System;

namespace Battles.Characters
{
    [Serializable]
    sealed class ShadowPriest : Character
    {
        public ShadowPriest(string name)
            : base(name, "ShadowPriest")
        {
            Skills.Add(new MindFlay());
            Skills.Add(new MindBlast());
            Skills.Add(new ShadowEmbrace());
            Skills.Add(new VampiricTouch());
        }
    }
}
