using System;

namespace Battles
{
    sealed class LootMenu : Menu
    {
        private static readonly string[] optionsInit = { "View loot", "Drop item", "Continue" };

        public LootMenu() 
            : base(optionsInit, "")
        {
            actions[0] = delegate // View Loot
            {
                ShowSingleActionMenu(
                    Battle.Player.LootItems,
                    Item.LootAction,
                    "Loot",
                    "There is no loot left.");
            };

            actions[1] = delegate // Drop item
            {
                ShowSingleActionMenu(
                    Battle.Player.OwnerUnit.Inventory,
                    Item.DropAction,
                    "Drop items",
                    Item.ZeroItemsError
                    );
            };
        }
    }
}
