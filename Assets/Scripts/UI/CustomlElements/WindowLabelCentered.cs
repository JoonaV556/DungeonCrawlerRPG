using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Draggable window with centered label
/// </summary>
public class WindowLabelCentered : DraggableWindow
{
    public Label WindowLabel { get; protected set; }

    public WindowLabelCentered() {
        closeWindowButton.AddToClassList("headerItem-rightAnchored");
        Header.AddToClassList("header-centerContent");
        WindowLabel = UIShortcuts.CreateVisualElement<Label>(Header, "window-header-label");
        WindowLabel.text = "Window";
    }

    public new class UxmlFactory : UxmlFactory<WindowLabelCentered> { }
}
