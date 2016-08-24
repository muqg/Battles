using System;

namespace Battles
{
    sealed class SkillMenu : Menu
    {
        private static string[] optionsInit = { "Info", "Level up", "Back" };

        public SkillMenu()
            : base(optionsInit)
        {
            actions[0] = delegate // Info
            {
                Console.WriteLine(Game.CurrentSkill.Description());
                Announce(Game.CurrentSkill.ToString());
            };

            actions[1] = delegate // Level up
            {
                if(Game.CurrentCharacter != null)
                {
                    Game.CurrentCharacter.LevelSkill(Game.CurrentSkill);
                }
                Announce(Game.CurrentSkill.ToString());
            };
        }
    }
}
