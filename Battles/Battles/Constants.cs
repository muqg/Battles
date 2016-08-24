namespace Battles
{
    static class Constants
    {
        // Message and command constants
        internal const string InvalidCommand = "Invalid command.\n";
        internal const string IntCommandPattern = @"\d+";
        internal const string AlphaCommandPattern = @"\w+";
        internal const string AlphaNumbericCommandPattern = @"[\w\d]+";
        internal const string CommandPattern = @".+";
        internal const string BackMessage = "(b to go back)";
        internal const string BackCommand = "b";
        internal const string ItemUsedMessage = "An item has already been used this turn.\n";

        // Character constants
        internal const int DefaultHealth = 300;
        internal const int DefaultMana = 100;
        internal const int DefaultManaRegeneration = 10;
        internal const int DefaultAttack = 10;
        internal const int DefaultSpellpower = 0;
        internal const float DefaultHaste = 100;
        internal const int MinArmour = -10;
        internal const int MaxCharacters = 6;
        internal const int MaxLevel = 10;
        internal const int UltimateLevel = 6;
        internal const int LevelExperienceBase = 11;

        // Inventory and items
        internal const int InventoryLimit = 42;
        internal const int MaxEquippedItems = 6;
    }
}
