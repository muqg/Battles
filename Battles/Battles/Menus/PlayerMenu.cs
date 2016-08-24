namespace Battles
{
    sealed class PlayerMenu : Menu
    {
        static string[] optionsInit = { "Battle", "Characters", "Logout" };
        static PlayerCharactersMenu playerCharacterMenu = new PlayerCharactersMenu();

        public PlayerMenu()
            : base(optionsInit, $"Hello, {Game.CurrentPlayer.Name}")
        {
            actions[0] = delegate // Battle
            {
                ShowSingleActionMenu(
                    Game.CurrentPlayer.Characters,
                    Character.SelectMenuAction,
                    "Select character for battle",
                    Character.NoCharactersError
                    );
            };

            actions[1] = delegate // Characters
            {
                playerCharacterMenu.Show();
            };
        }
    }
}
