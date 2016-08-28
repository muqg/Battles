using System;
using System.Collections.Generic;

namespace Battles
{
    class Stats
    {
        private int _maxHealth;
        private int _health;
        private int _attack;
        private int _armour;
        private float _currentHaste;
        private float _haste;

        public Stats(Unit ownerUnit)
        {
            OwnerUnit = ownerUnit;

            MaxHealth = OwnerUnit.Health; // Setter of MaxHealth also sets Health since health is increasing from 0 to a value
            HealthRegen = OwnerUnit.HealthRegeneration;
            Attack = OwnerUnit.Attack;
            Armour = OwnerUnit.Armour;
            CurrentHaste = Haste = OwnerUnit.Haste;
        }

        public int HealthRegen { get; set; }
        public List<Buff> Buffs { get; private set; } = new List<Buff>();
        public Unit OwnerUnit { get; }

        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                int difference = value - _maxHealth;
                _maxHealth = value;
                if (difference > 0) // Add the increase of the maximum health to the current Health
                    Health += difference;
            }
        }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value.Clamp(0, MaxHealth);
            }
        }
        public int Attack
        {
            get
            {
                return _attack;
            }
            set
            {
                _attack = Math.Max(0, value);
            }
        }
        public int Armour
        {
            get
            {
                return _armour;
            }
            set
            {
                _armour = Math.Max(Constants.MinArmour, value);
            }
        }
        public float CurrentHaste
        {
            get
            {
                return _currentHaste;
            }
            set
            {
                // Minimum of 1 so that unit can always act
                _currentHaste = value.Clamp(1, Haste);
            }
        }
        public float Haste
        {
            get
            {
                return _haste;
            }
            set
            {
                // Minimum of 1 so that unit can always act
                // Always update current haste
                _haste = Math.Max(1, value);
            }
        }

        public void ShowBuffs()
        {
            Menu.Announce(OwnerUnit.Name + "'s buffs");
            if (Buffs.Count > 0)
            {
                foreach (Buff b in Buffs)
                    Console.WriteLine(b);
            }
            else
            {
                Console.WriteLine("No buffs");
            }
            Console.WriteLine();
        }

        public override string ToString()
        {
            Menu.Announce(OwnerUnit.Name + "'s stats");
            return string.Join("\n",
                $"Health: {Health}/{MaxHealth}",
                $"Attack: {Attack}",
                $"Armour: {Armour}",
                $"Haste: {Haste}",
                $"Health Regeneration: {HealthRegen}",
                "");
        }

        // Used for checking the duration and effect of all buffs at the end of an action turn
        public void CheckBuffs()
        {
            int index = 0;
            while(index < Buffs.Count)
            {
                if (Buffs[index].CheckDuration()) // Check if buff duration (wether it is still applied); true = applied; false = expired
                    index += 1;
            }
        }
    }
}
