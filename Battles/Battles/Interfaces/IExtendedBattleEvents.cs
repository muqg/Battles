namespace Battles
{
    interface IExtendedBattleEvents : IBattleEvents
    {
        // Called on the unit that is attacking when starting the attack
        // Returns false on impaired attack (e.g. disarmed)
        bool OnAttackUse(CharacterStats player, Stats enemy, EffectValues values);

        // Called on the unit that is using the skill when triggering the skill
        // Returns false on impaired usage (e.g. silenced)
        bool OnSkillUse(CharacterStats player, Stats enemy, EffectValues values);
    }
}
