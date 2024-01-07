using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot {

    public InventoryItem HeldItem;

    /// <summary>
    /// Checks whether or not the slot has an item innit. If yes, also gives the InventoryItem and its type.
    /// </summary>
    /// <param name="invItem">Inventory item in the slot, if the slot has one. </param>
    /// <param name="itemType">Type of the item in the slot, if the slot has one. </param>
    /// <returns>True if the slot has an item innit. False if not.</returns>
    public bool HasItem(out InventoryItem invItem, out Item itemType) {
        if (HeldItem != null) {
            invItem = HeldItem;
            itemType = HeldItem.Type;
            return true;
        }
        invItem = null;
        itemType = null;
        return false;
    }
    public bool HasItem() {
        if (HeldItem != null) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to add an item into the slot. If slot has an item innit, tries to increase the stack size
    /// </summary>
    /// <param name="ItemToAdd"></param>
    /// <returns></returns>
    public bool TryToAddItem(InventoryItem ItemToAdd) {
        // Add the item to the slot if it doesn't have anything innit
        if (!HasItem()) {
            HeldItem = ItemToAdd;
            return true;
        }

        // Check if items are of same type
        bool IsSameType = HeldItem.Type == ItemToAdd.Type;

        // Cannot be stacked
        if (!IsSameType) { return false; }

        // Check if stack size can be increased
        bool CanIncreaseStackSize = ItemToAdd.StackSize + HeldItem.StackSize <= HeldItem.Type.MaxStackSize;

        // Stack size exceeds limit
        if (!CanIncreaseStackSize) { return false; }

        // Increase stack size
        HeldItem.StackSize += ItemToAdd.StackSize;
        return true;
    }
}
