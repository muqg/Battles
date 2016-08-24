namespace Battles
{
    interface IBattleEvents
    {
        // Called on the target unit of the attack
        bool OnAttackHit(Stats self, Stats attacker, EffectValues effect);

        // Called on the target unit of the skill
        bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues effect);
    }
}
