using System;
using System.Collections.Generic;
using System.Linq;

namespace Battles
{
    sealed class ItemMenu : Menu
    {
        private static readonly string[] optionsInit = { "Inventory", "Equipped Items", "Equip", "Unequip", "Drop Items", "Back" };

        public ItemMenu()
            : base(optionsInit, $"{Game.CurrentCharacter.Name}'s items")
        {
            actions[0] = inventoryAction; // Inventory

            actions[1] = equippedItemsAction; // Equipped items

            actions[2] = equipAction; // Equip

            actions[3] = unequipAction; // Unequip

            actions[4] = dropAction; // Drop items
        }

        private static Action inventoryAction = delegate
        {
            ShowSingleActionMenu(
                Game.CurrentCharacter.Inventory,
                Item.IntenvtoryAction,
                $"{Game.CurrentCharacter.Name}'s inventory",
                Item.ZeroItemsError
                );
        };

        private static Action equippedItemsAction = delegate
        {
            ShowSingleActionMenu(
                Game.CurrentCharacter.Items,
                Item.IntenvtoryAction,
                $"{Game.CurrentCharacter.Name}'s equipment",
                "You have no equipped items"
                );
        };

        private static Action equipAction = delegate
        {
            List<Item> equippableItems = Game.CurrentCharacter.Inventory
                .Where(i => i.Type == Item.ItemType.Normal || i.Type == Item.ItemType.Usable).ToList();

            ShowSingleActionMenu(
                equippableItems,
                Item.EquipAction,
                "Select an item to equip",
                "There are no equippable items to choose from");
        };

        private static Action unequipAction = delegate
        {
            ShowSingleActionMenu(
                Game.CurrentCharacter.Items,
                Item.UneqipAction,
                "Select an item to unequip",
                "You have no equipped items");
        };

        private static Action dropAction = delegate
        {
            ShowSingleActionMenu(
                Game.CurrentCharacter.Inventory,
                Item.DropAction,
                "Drop items",
                Item.ZeroItemsError
                );
        };
    }
}
