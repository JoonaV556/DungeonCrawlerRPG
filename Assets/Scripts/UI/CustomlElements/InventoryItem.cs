using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Visual UI representation of an item inside an inventory slot. Every slot has an InventoryItem as its child regarldess of the slot actually having an item in it
/// </summary>
public class InventoryItem : VisualElement
{
    /// <summary>
    /// Black background behind the stack size number
    /// </summary>
    private VisualElement stackSizeContainer;
    /// <summary>
    /// Number that represents the items stack size
    /// </summary>
    private Label stackSizeLabel;

    // TODO - Move to reusable parent class
    public float sizeX { get; private set; } // InventoryItems approximate width on screen in pixels
    public float sizeY { get; private set; } // InventoryItems approximate width on screen in pixels

    // Factory for UIBuilder compatibility
    public new class UxmlFactory : UxmlFactory<InventoryItem> { }

    // Constructor
    public InventoryItem() {
        // Create child members
        this.AddToClassList("item-image");
        stackSizeContainer = UIShortcuts.CreateVisualElement<VisualElement>(this, "item-stacksizeContainer");
        stackSizeLabel = UIShortcuts.CreateVisualElement<Label>(stackSizeContainer, "item-stacksizeLabel");
        stackSizeLabel.text = "1";

        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        // Hide item by default
        // ConstructUI.HideElement(this);
    }

    // Triggered when the elements geometry changes... :D
    private void OnGeometryChanged(GeometryChangedEvent evt) {
        // Update Items approximate size
        sizeX = this.resolvedStyle.width;
        sizeY = this.resolvedStyle.height;
    }
}
