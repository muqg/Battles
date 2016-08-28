using System;

namespace Battles.Enemies
{
    sealed class MechanicalYeti : Enemy
    {
        private const string name = "Mechanical Yeti";
        private const string yetiNamespace = "Battles.MechanicalYeti";

        private static Type[] spareParts = Utility.GetTypes(yetiNamespace);

        public MechanicalYeti()
            : base(name, 500, 40, 4, armour: 4)
        {
        }

        public override void Initialize(CharacterStats player, Stats enemy)
        {
            upgrade(player, enemy);
            base.Initialize(player, enemy);
        }

        protected override void EndTurn(CharacterStats player, Stats self)
        {
            base.EndTurn(player, self);
            upgrade(player, self);
        }

        private void upgrade(CharacterStats player, Stats enemy)
        {
            Stats target;
            if (random.Next(5) == 0)
                target = enemy;
            else
                target = player;

            int sparePart = random.Next(0, spareParts.Length);
            Buff buff = Buff.AddBuff(Activator.CreateInstance(spareParts[sparePart]) as Buff, target);
            buff.SetStacks();

            Menu.Announce($"{Name} gives {buff.Name} to {target.OwnerUnit.Name}");
        }
    }
}
