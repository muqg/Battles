using System;

namespace Battles
{
    [Serializable]
    sealed class ChaosStrike : Skill
    {
        private const string name = "Chaos Strike";
        private const int cost = 4;

        private static SoulShard shard;

        public ChaosStrike()
            : base(name, SkillType.Attack, cost)
        {
        }

        private int attackModifier => 1 + Level / 4;
        private int shardModifier => Level;

        public override void Refresh()
        {
            shard = new SoulShard();
            base.Refresh();
        }

        protected override int Power(CharacterStats player, Stats enemy)
        {
            shard = Buff.AddBuff<SoulShard>(player.Buffs);

            return player.Attack * attackModifier + shard.Stacks * shardModifier - enemy.Armour;
        }

        protected override void SkillEffect(CharacterStats player, Stats enemy)
        {
            player.Health -= SkillEffectValues.Damage / 2;
            Battle.WritePlayerDamage(name, "effect", SkillEffectValues.Damage / 2, player.Health);
        }

        protected override string SpecificBattleDescription(CharacterStats player, Stats enemy)
        {
            int power = Power(player, enemy);

            return $"Deals ({power}) damage to your enemy and ({power / 2}) damage to you.";
        }

        protected override string SpecificDescription() => $"A powerful attack that damages your enemy and you for half that amount. Damage increases for each {shard.Name}."
            + "\nLevels increase both attack and shard bonus damage.";
    }
}
