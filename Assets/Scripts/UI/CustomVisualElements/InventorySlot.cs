using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    private bool mouseDown = false;

    public InventoryItem childItem;


    public InventorySlot() {
        childItem = ConstructUI.CreateVisualElement<InventoryItem>(this);
        ConstructUI.HideElement(childItem);

        // Register callbacks
        RegisterCallback<MouseDownEvent>(OnMouseDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
        RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
    }

    private void OnMouseDown(MouseDownEvent evt) {
        mouseDown = true;
    }

    private void OnMouseLeave(MouseLeaveEvent evt) {  mouseDown = false; }

    private void OnMouseMove(MouseMoveEvent evt) {
        // Check if mouse is pressed down
        if (!mouseDown) {
            return;
        }
        // Check if slot has an item
        if (HasItem(out Background image)) {
            InventoryHUD.Instance.StartDrag(image);
        }
    }

    // Does this slot have an item innit?
    public bool HasItem(out Background itemImage) {
        bool hasItem = ConstructUI.IsEnabled(childItem);
        if (hasItem) {
            itemImage = childItem.style.backgroundImage.value;
        } else {
            itemImage = null;
        }
        return hasItem;
    }
}
