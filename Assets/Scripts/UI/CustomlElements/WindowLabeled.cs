using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Draggable window with a label acnhored to the left
/// </summary>
public class WindowLabeled : DraggableWindow
{
    public Label WindowLabel { get; protected set; }

    public new class UxmlFactory : UxmlFactory<WindowLabeled> { }

    public WindowLabeled() {
        WindowLabel = UIShortcuts.CreateVisualElement<Label>(Header, "window-header-label");
        WindowLabel.text = "Window";
    }
}
