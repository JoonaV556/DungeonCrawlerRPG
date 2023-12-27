using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement {
    private bool mouseDown = false;

    public InventoryItem childItem;


    public InventorySlot() {
        childItem = ConstructUI.CreateVisualElement<InventoryItem>(this);
        ConstructUI.HideElement(childItem);

        // Register callbacks
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
        RegisterCallback<MouseEnterEvent>(OnMouseEnter);

    }

    private void OnMouseEnter(MouseEnterEvent evt) {
        // Inform inventoryHUD that mouse is over a slot (Used for dragging stuff)
        InventoryHUD.Instance.OnCursorEnterSlot(this);

    }

    private void OnMouseLeave(MouseLeaveEvent evt) {
        // Inform inventoryHUD that mouse has exited the slot (Used for dragging stuff)
        InventoryHUD.Instance.OnCursorLeaveSlot(this);

        mouseDown = false;
    }

    private void OnMouseDown(MouseDownEvent evt) {
        mouseDown = true;
    }

    private void OnMouseMove(MouseMoveEvent evt) {
        // Check if mouse is pressed down
        if (!mouseDown) {
            return;
        }
        // Check if slot has an item
        if (HasItem(out Background image)) {
            InventoryHUD.Instance.StartDrag(this, image);
        }
    }

    // Does this slot have an item innit?
    public bool HasItem(out Background itemImage) {
        // Check if the slot has an item
        bool hasItem = ConstructUI.IsEnabled(childItem);
        if (hasItem) {
            // If the slot has an item, return the item in the slot 
            itemImage = childItem.style.backgroundImage.value;
        } else {
            itemImage = null;
        }
        return hasItem;
    }

    public bool HasItem() {
        // Check if the slot has an item
        bool hasItem = ConstructUI.IsEnabled(childItem);
        return hasItem;
    }

    public void SetItem(Texture2D newItem) {
        ConstructUI.ShowElement(childItem);
        childItem.style.backgroundImage = newItem;
    }

    // Removes the current item in the slot, if one exists
    public void ClearItem() {
        if (this.HasItem()) {
            ConstructUI.HideElement(childItem);
            childItem.style.backgroundImage = null;
        }
    }
}
