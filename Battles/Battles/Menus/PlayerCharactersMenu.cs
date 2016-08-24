namespace Battles
{
    sealed class PlayerCharactersMenu : Menu
    {
        static string[] optionsInit = { "Create", "View", "Delete", "Back" };

        public PlayerCharactersMenu()
            : base(optionsInit, "Characters")
        {
            actions[0] = delegate // Create
            {
                Character.Create();
            };

            actions[1] = delegate // View
            {
                ShowSingleActionMenu(
                    Game.CurrentPlayer.Characters,
                    Character.ViewMenuAction,
                    "Your characters",
                    Character.NoCharactersError);
            };

            actions[2] = delegate // Delete
            {
                ShowSingleActionMenu(
                    Game.CurrentPlayer.Characters,
                    Character.DeleteMenuAction,
                    "Delete character",
                    Character.NoCharactersError);
            };
        }
    }
}
