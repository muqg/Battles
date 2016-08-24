namespace Battles
{
    sealed class MainMenu : Menu
    {
        static string[] optionsInit = { "New", "Login", "Exit" };

        public MainMenu()
            : base(optionsInit, "Welcome to Battles project!")
        {
            actions[0] = delegate // New
            {
                Announce("Create new account");
                Game.CurrentPlayer = Account.New();
                showPlayerMenu();
            };
            actions[1] = delegate // Login
            {
                Announce("Log in");
                Game.CurrentPlayer = Account.Login();
                showPlayerMenu();
            };
        }

        static void showPlayerMenu()
        {
            if(Game.CurrentPlayer != null)
                new PlayerMenu().Show();
        }
    }
}
