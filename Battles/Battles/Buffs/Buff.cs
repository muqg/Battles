using System;
using System.Collections.Generic;
using System.Text;

namespace Battles
{
    class Buff : IExtendedBattleEvents
    {
        protected int _stacks = 0;

        #region Constructors

        public Buff(string name = "Unknown",
            int health = 0,
            int healthRegeneration = 0,
            int mana = 0,
            int manaRegeneration = 0,
            int attack = 0,
            int armour = 0,
            float haste = 0,
            int spellPower = 0,
            int maxStacks = int.MaxValue, 
            int duration = -1,
            bool dispellable = true)
        {
            Name = name;
            Health = health;
            HealthRegeneration = Math.Max(0, healthRegeneration);
            Mana = mana;
            ManaRegeneration = Math.Max(0, manaRegeneration);
            Attack = attack;
            Armour = armour;
            Haste = haste;
            SpellPower = spellPower;
            MaxStacks = Math.Max(0, maxStacks);
            CurrentDuration = Duration = duration;
            Dispellable = dispellable;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; }
        public bool Dispellable { get; }

        protected int Health { get; } // Affects MaxHealth 
        protected int HealthRegeneration { get; }
        protected int Mana { get; } // Affects MaxMana
        protected int ManaRegeneration { get; }
        protected int Attack { get; }
        protected int Armour { get; }
        protected float Haste { get; }
        protected int SpellPower { get; }
        protected int MaxStacks { get; }
        protected int Duration { get; }
        protected int CurrentDuration { get; set; }

        public int Stacks
        {
            get
            {
                return _stacks;
            }
            private set
            {
                _stacks = value.Clamp(0, MaxStacks);
            }
        }

        #endregion Properties

        // Adds a buff of existing type that is not initialized or returns an already applied one
        public static T AddBuff<T>(List<Buff> buffs) where T : Buff, new()
        {
            T appliedBuff = GetBuff<T>(buffs);
            if (appliedBuff == null) // Add buff if not already preset and return it
            {
                T buff = new T();
                buffs.Add(buff);
                return buff;
            }
            else
                return appliedBuff; // Return already present buff
        }

        // Adds a buff of non-existing (dynamic) type, an existing type with parameters that is pre-initialized or returns an already applied one
        public static Buff AddBuff(Buff buff, List<Buff> buffs)
        {
            Buff appliedBuff = GetBuff(buff.Name, buffs);
            if(appliedBuff == null) // Add buff if not already present
            {
                buffs.Add(buff);
                return buff;
            }

            return appliedBuff; // Return already present buff
        }

        public virtual bool OnAttackHit(Stats self, Stats attacker, EffectValues attackValue)
        {
            return true;
        }

        public virtual bool OnAttackUse(CharacterStats player, Stats enemy, EffectValues attackValues)
        {
            return true;
        }

        public virtual bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues skillValues)
        {
            return true;
        }

        public virtual bool OnSkillUse(CharacterStats player, Stats enemy, EffectValues skillValues)
        {
            return true;
        }

        protected virtual void Effect(Stats stats) { }

        public override string ToString()
        {
            return $"{Name} - {SpecificDescription()}";
        }

        // Adds or removes stacks and manages corresponding stats
        public void SetStacks(Stats stats, int count = 1)
        {
            if (stats.Buffs.Contains(this)) // Make sure to not add stacks to stats without this buff
            {
                CurrentDuration = Duration; // Always refresh duration when applying stacks

                if (count + Stacks >= MaxStacks)
                    count = MaxStacks - Stacks;
                else if (count + Stacks < 0)
                    count = Stacks;

                Stacks += count;

                stats.MaxHealth += count * Health; // Setting MaxHealth affects Health as well
                stats.HealthRegen += count * HealthRegeneration;
                stats.Attack += count * Attack;
                stats.Armour += count * Armour;
                stats.Haste += count * Haste; // Set only MaxHaste here

                if (stats is CharacterStats)
                {
                    CharacterStats cs = stats as CharacterStats;

                    cs.MaxMana += count * Mana; // Setting MaxMana affects Mana as well
                    cs.ManaRegen += count * ManaRegeneration;
                    cs.SpellPower += count * SpellPower;
                }
            }
        }

        // Removes the buff
        public void Remove(Stats stats)
        {
            SetStacks(stats, -Stacks); // Remove buff bonuses to the stats
            stats.Buffs.Remove(this);
        }

        // Checks the duration of the buff and uses its effect
        public bool CheckDuration(Stats stats)
        {         
            // NOTE: Negative Duration means infinite or until removed in a special manner

            if (CurrentDuration > 0)
            {
                // Use buff's effect
                Effect(stats);
                CurrentDuration = Math.Max(0, CurrentDuration - 1); // Limits duration to 0
            }

            if (CurrentDuration == 0)
            {
                Remove(stats);
                return false;
            }

            // Returns true if buff is still applied. Returns false if buff has expired (has been removed)
            return true;
        }

        public void WriteStacks()
        {
            Console.WriteLine($"You have {Stacks} stacks of {Name}.\n".Indent());
        }

        public void WriteEnemyStacks(string enemyName)
        {
            Console.WriteLine($"{enemyName} has {Stacks} stacks of {Name}.\n".Indent());
        }

        protected virtual string Description() => "Unknown";

        protected string SpecificDescription()
        {
            StringBuilder sb = new StringBuilder();
            // General description
            sb.Append(Description());

            // Duration
            if (CurrentDuration > 0)
                sb.Append($"; Turns left: {CurrentDuration}");

            // Stacks
            if (MaxStacks > 1 && Stacks > 0)
                sb.Append($"; Stacks: {Stacks}");
            if (MaxStacks > 1 && MaxStacks != int.MaxValue)
                sb.Append($"/{MaxStacks}");

            return sb.ToString();
        }

        // Returns buff of type T. It is added if not already present
        private static T GetBuff<T>(List<Buff> buffs) where T : Buff, new()
        {
            foreach (Buff b in buffs)
            {
                if (b is T)
                    return b as T; // Return existing buff
            }

            return null; // Buff is not present
        }

        // Gets a buff by name
        private static Buff GetBuff(string name, List<Buff> buffs)
        {
            return buffs.Find(b => b.Name.Equals(name));
        }
    }
}
