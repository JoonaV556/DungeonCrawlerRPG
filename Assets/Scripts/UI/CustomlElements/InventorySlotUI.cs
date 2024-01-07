using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlotUI : VisualElement {

    /// <summary>
    /// The visual reperesentation of an item in the slot. Slot always has one. When slot actually doesn't have an item innit, the childItem is just hidden
    /// </summary>
    public InventoryItemUI childItem;

    /// <summary>
    /// Whether or not mouse is currently held down over this slot?
    /// </summary>
    private bool mouseDown = false;

    // Constructor
    public InventorySlotUI() {
        // Create the childItem
        childItem = UIShortcuts.CreateVisualElement<InventoryItemUI>(this);
        // Hide the childItem by default
        UIShortcuts.HideElement(childItem);

        // Register callbacks to mouse events
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        RegisterCallback<MouseEnterEvent>(OnMouseEnter);

    }

    #region RespondToMouseInput

    private void OnMouseEnter(MouseEnterEvent evt) {
        // Inform inventoryHUD that mouse is over a slot (Used for dragging stuff)
        InventoryHUD.Instance.OnCursorEnterSlot(this);
    }

    private void OnMouseLeave(MouseLeaveEvent evt) {
        // Inform inventoryHUD that mouse has exited the slot (Used for dragging stuff)
        InventoryHUD.Instance.OnCursorLeaveSlot(this);
        // Update mouseDown
        mouseDown = false;
    }

    private void OnMouseDown(MouseDownEvent evt) {
        // Update mouseDown
        mouseDown = true;
    }

    private void OnMouseMove(MouseMoveEvent evt) {
        // Check if mouse is pressed down
        if (!mouseDown) {
            return;
        }
        // Check if slot has an item
        if (HasItem(out Background image)) {
            // Inform inventory manager that item is being dragged from this slot
            InventoryHUD.Instance.StartDrag(this, image);
        }
    }

    #endregion

    /// <summary>
    /// Checks if this slot has an item init. If yes, the item is returned
    /// </summary>
    /// <param name="itemImage">Item currently in the slot</param>
    /// <returns>Returns true if the slot has an item in it, else false</returns>
    public bool HasItem(out Background itemImage) {
        // Check if the slot has an item
        bool hasItem = UIShortcuts.IsEnabled(childItem);
        if (hasItem) {
            // If the slot has an item, return the item in the slot 
            itemImage = childItem.style.backgroundImage.value;
        } else {
            // Else return nothing
            itemImage = null;
        }
        return hasItem;
    }

    /// <summary>
    /// Checks if this slot has an item init.
    /// </summary>
    /// <returns>Returns true if the slot has an item in it, else false</returns>
    public bool HasItem() {
        // Check if the slot has an item
        bool hasItem = UIShortcuts.IsEnabled(childItem);
        return hasItem;
    }

    /// <summary>
    /// Sets the item in the InventorySlot. Warning: If item already exists in the slot, it will not be removed! Rememeber to remove the item beforehand
    /// </summary>
    /// <param name="newItem">New item to add into the slot</param>
    public void SetItem(Texture2D newItem) {
        // Enable the itemUI 
        UIShortcuts.ShowElement(childItem);
        // Set the new item
        childItem.style.backgroundImage = newItem;
    }

    /// <summary>
    /// Removes the item currently in the slot
    /// </summary>
    public void ClearItem() {
        if (this.HasItem()) {
            // Make the itemUI not visible
            UIShortcuts.HideElement(childItem);
            // Clear the item
            childItem.style.backgroundImage = null;
        }
    }

    public new class UxmlFactory : UxmlFactory<InventorySlotUI> { }
}
