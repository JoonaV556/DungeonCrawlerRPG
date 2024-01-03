using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestHUD : ConstructUI {

    [SerializeField]
    private StyleSheet closeBtnStyle;
    [SerializeField]
    private StyleSheet windowStyle;
    [SerializeField]
    private StyleSheet slotStyle;

    [SerializeField]
    private StyleSheet[] styleSheets;

    private CloseWindowButton closeButton;
    private DraggableWindow window;

    protected override void Construct() {
        RootElement.AddToClassList("root");
        RootElement.AddToClassList("droppable");

        foreach (var style in styleSheets) {
            RootElement.styleSheets.Add(style);
        }

        // test creating a window
        TestWindow();
    }

    private void TestWindow() {
        // Create parent window
        window = UIShortcuts.CreateVisualElement<DraggableWindow>(RootElement);
        window.styleSheets.Add(windowStyle);

        // Test adding content to the window
        RootElement.styleSheets.Add(slotStyle);
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
        window.AddContent(UIShortcuts.CreateVisualElement<InventorySlot>(null, "item-slot"));
    }
}
