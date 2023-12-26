using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryItem : VisualElement
{
    private VisualElement stackSizeContainer;
    private Label stackSizeLabel;

    public float sizeX { get; private set; }
    public float sizeY { get; private set; }

    public new class UxmlFactory : UxmlFactory<InventoryItem> { }

    public InventoryItem() {
        // Create child members
        this.AddToClassList("item-image");
        stackSizeContainer = ConstructUI.CreateVisualElement<VisualElement>(this, "item-stacksizeContainer");
        stackSizeLabel = ConstructUI.CreateVisualElement<Label>(stackSizeContainer, "item-stacksizeLabel");
        stackSizeLabel.text = "1";

        RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);

        // Hide item by default
        // ConstructUI.HideElement(this);
    }

    private void OnGeometryChanged(GeometryChangedEvent evt) {
        sizeX = this.resolvedStyle.width;
        sizeY = this.resolvedStyle.height;
    }
}
