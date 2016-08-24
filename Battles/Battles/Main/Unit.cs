using System;

namespace Battles
{
    [Serializable]
    abstract class Unit : IBattleEvents
    {
        protected static Random random = new Random();

        public Unit(string name, int health, int attack, float haste, int level = 1, int armour = 0, int healthRegeneration = 0)
        {
            Name = name;
            Level = level;
            Health = health;
            Attack = attack;
            Armour = armour;
            Haste = haste;
            HealthRegeneration = healthRegeneration;
        }

        public string Name { get; protected set; }
        public int Level { get; protected set; }
        public int Health { get; protected set; }
        public int Attack { get; protected set; }
        public int Armour { get; protected set; }
        public float Haste { get; protected set; }
        public int HealthRegeneration { get; protected set; }

        protected abstract void EndTurn(CharacterStats player, Stats enemy);

        public virtual void Act(CharacterStats player, Stats enemy)
        {
            // Always call this or base when overriding
            EndTurn(player, enemy);
        }

        // Used for general initialization prior to battle start -> stat resets, skill refreshing and other preparations
        public virtual void Initialize(CharacterStats player, Stats enemy) { }

        // Checks all buff and item effects on the attacked unit
        // Returns false on impaired by an effect attack; True on successful attack
        public virtual bool OnAttackHit(Stats self, Stats attacker, EffectValues attackValues) // Always call when overriding
        {
            foreach(Buff buff in self.Buffs) // Check buff effects for attacked unit
            {
                if (!buff.OnAttackHit(self, attacker, attackValues))
                    return false;
            }

            if(self is CharacterStats) // Player is attacked --> check item effects
            {
                CharacterStats player = self as CharacterStats;
                foreach (Item item in player.OwnerUnit.Items)
                    if (!item.OnAttackHit(player, attacker, attackValues)) return false;
            }

            return true;
        }

        public virtual bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues skillEffectValues)
        {
            return true;
        }

        public void AttackEnemy(Stats self, Stats target)
        {
            EffectValues attackValues = new EffectValues(self.Attack);
            CharacterStats player = null;
            if (self is CharacterStats) // Check if player is attacking
                player = self as CharacterStats;

            // Check buff and item effects on attacking unit
            if (player != null) // Player is attacking
            {
                foreach (Buff buff in self.Buffs) // Buff events
                    if (!buff.OnAttackUse(player, target)) return;
                foreach (Item item in player.OwnerUnit.Items) // Item events
                    if (!item.OnAttackUse(player, target)) return;
            }
            else // Enemy is attacking
            {
                foreach (Buff buff in self.Buffs)
                    if (!buff.OnAttackUse((CharacterStats)target, self)) return;
            }

            // Calculate attack value and check buff and item effects
            if (target.OwnerUnit.OnAttackHit(target, self, attackValues))
            {
                int damage = attackValues.Damage - target.Armour;
                target.Health -= damage;

                if (player != null) // Player is attacking
                {
                    Battle.WriteEnemyHealth("Your attack", damage, target);
                }
                else // Enemy is attacking
                {
                    Battle.WritePlayerDamage(self.OwnerUnit.Name, "attack", damage, target.Health);
                }
            }
        }
    }
}
