using System;

namespace Battles
{
    sealed class CharacterMenu : Menu
    {
        private static readonly string[] optionsInit = { "Skills", "Items", "Info", "Back" };

        public CharacterMenu() : 
            base(optionsInit)
        {
            actions[0] = delegate // Skills
            {
                ShowSingleActionMenu(
                    Game.CurrentCharacter.Skills,
                    Skill.SkillAction, 
                    $"{Game.CurrentCharacter.Name}'s skills",
                    Skill.ZeroSkillsError);
                Announce(Game.CurrentCharacter.Name);
            };

            actions[1] = delegate // Items
            {
                new ItemMenu().Show();
            };

            actions[2] = delegate // Info
            {
                Console.WriteLine(Game.CurrentCharacter?.Description());
                Announce(Game.CurrentCharacter.Name);
            };
        }
    }
}
