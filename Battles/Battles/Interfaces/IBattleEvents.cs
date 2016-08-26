namespace Battles
{
    interface IBattleEvents
    {
        // Called on the target unit of the attack; Returns true if attack should procees; False if it is cacelled somehow
        bool OnAttackHit(Stats self, Stats attacker, EffectValues effect);

        // Called on the target unit of the skill; Returns true if skill should preceeed; False if it is cancelled somehow
        bool OnSkillHit(CharacterStats player, Stats enemy, EffectValues effect);
    }
}
