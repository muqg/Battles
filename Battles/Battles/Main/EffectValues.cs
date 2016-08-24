using System;

namespace Battles
{
    [Serializable]
    class EffectValues
    {
        int _damage;
        int _healing;

        public EffectValues(int damage = -1, int healing = -1, Skill source = null)
        {
            _damage = damage;
            _healing = healing;
            SourceSkill = source;
        }

        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = Math.Max(0, value);
            }
        }
        public int Healing
        {
            get
            {
                return _healing;
            }
            set
            {
                _healing = Math.Max(0, value);
            }
        }
        public Skill SourceSkill { get; set; }
    }
}