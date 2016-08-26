using System;

namespace Battles
{
    [Serializable]
    abstract class Skill : ISingleActionMenu
    {
        protected static Random random = new Random();
        protected static SkillMenu skillMenu = new SkillMenu();

        public Skill(string name, SkillType type, int cost, bool isUltimate = false, int cooldown = 0)
        {
            Name = name;
            Type = type;
            Cost = cost;
            Cooldown = cooldown;
            IsUltimate = isUltimate;

            if(isUltimate)
            {
                Level = 0;
                MaxLevel = 1;
            }
            else
            {
                Level = 1;
                MaxLevel = 4;
            }

            Refresh(); // For when loading a skill
        }

        public static Action<Skill> SkillAction = delegate(Skill s)
        {
            Game.CurrentSkill = s;
            Menu.Announce(s.ToString());
            skillMenu.Show();
            Game.CurrentSkill = null;
        };

        public enum SkillType
        {
            Unknown, Attack, Magic, Mixed
        }

        public static string ZeroSkillsError = "Skills for this class are Not Yet Implemented (NYI)";

        public string Name { get; }
        public bool IsUltimate { get; }
        public int Level { get; protected set; }
        public int Cost { get; }
        public int CurrentCooldown { get; set; }

        protected int Cooldown { get; }
        protected int MaxLevel { get; }
        protected SkillType Type { get; }
        protected EffectValues SkillEffectValues { get; set; }

        // Loads a skill morroring any changes to constants and adding new components to the saved one by creating a fresh instance
        public static Skill Load(Skill loadingSkill)
        {
            Skill skill = Activator.CreateInstance(loadingSkill.GetType()) as Skill;
            skill.Level = loadingSkill.Level.Clamp(0, skill.MaxLevel);

            return skill;
        }

        public string BattleDescription(CharacterStats player, Stats enemy)
        {
            return $"{Name} ({Cost} mana) - {SpecificBattleDescription(player, enemy)}{(CurrentCooldown > 0 ? "| Cooldown: " + CurrentCooldown : "")}";
        }

        public string Description() => string.Join("\n",
            $"{Name}, Level {Level}" + (IsUltimate ? " (Ultimate)" : ""),
            $"Type: {Type}",
            $"Cost: {Cost} mana",
            $"Cooldown: {Cooldown} turn" + (Cooldown == 1 ? "" : "s"),
            "",
            SpecificDescription(),
            "");

        // Returns true on successful usage
        public bool Use(CharacterStats player, Stats enemy)
        {
            if (SetSkillEffectValues(player, enemy))
            {
                foreach (Buff buff in player.Buffs) // Call buff effects
                    if (!buff.OnSkillUse(player, enemy, SkillEffectValues)) return false;
                foreach (Item item in player.OwnerUnit.Items) // Call item effects
                    if (!item.OnSkillUse(player, enemy, SkillEffectValues)) return false;

                if (enemy.OwnerUnit.OnSkillHit(player, enemy, SkillEffectValues))
                {
                    SkillEffect(player, enemy);
                    if(Cooldown > 0)
                        CurrentCooldown = Cooldown + 1; // +1 since one turn is immediately skipped at the end of this one
                }

                return true;
            }

            return false; // Failed usage --> not enough resources or other
        }

        // Resets necessary skill values and prepares skill for fresh use
        public virtual void Refresh()
        {
            SkillEffectValues = new EffectValues();
            CurrentCooldown = 0;
        }

        public override string ToString() => $"{Name}, level {Level}";

        public bool CheckCooldown()
        {
            if (CurrentCooldown <= 0)
                return true;

            string turns = CurrentCooldown == 1 ? "turn" : "turns";
            Console.WriteLine($"Skill is on cooldown for {CurrentCooldown} more {turns}.\n");
            return false;
        }

        // Levels the skill up
        public bool LevelUp()
        {
            if (Level < MaxLevel)
            {
                Level += 1;
                return true;
            }

            Console.WriteLine("Skill is already at max level.");
            return false;
        }

        // Uses the required by the skill mana. Returns false if there is not enough mana
        public bool UseMana(CharacterStats player)
        {
            if (player.Mana >= Cost)
            {
                player.Mana -= Cost;
                return true;
            }
            else
            {
                Console.WriteLine("Not enough mana.\n");
                return false;
            }
        }

        // Used to calculate the power of the skill
        protected abstract int Power(CharacterStats player, Stats enemy);

        // The actual effect of the skill (everything except usage conditions and damage/healing calculations)
        protected abstract void SkillEffect(CharacterStats player, Stats enemy);

        // Skill specific description for battle
        protected abstract string SpecificBattleDescription(CharacterStats player, Stats enemy);

        // Skill specific general description -> effect, usage and leveling
        protected abstract string SpecificDescription();

        // Sets effect values and checks casting conditions
        // Returns true on valid usage condition (e.g. player has enough soul shards to cast draw soul)
        protected virtual bool SetSkillEffectValues(CharacterStats player, Stats enemy)
        {
            SkillEffectValues = new EffectValues(Power(player, enemy), source: this);
            return true;
        }
    }
}