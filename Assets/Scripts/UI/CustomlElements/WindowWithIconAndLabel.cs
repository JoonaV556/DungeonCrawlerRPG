using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WindowWithIconAndLabel : DraggableWindow {
    public VisualElement Icon { get; protected set; }
    public VisualElement LeftContainer { get; protected set; }
    public Label WindowLabel { get; protected set; }


    public WindowWithIconAndLabel() {

        LeftContainer = UIShortcuts.CreateVisualElement<VisualElement>(Header, "header-left");
        
        Icon = UIShortcuts.CreateVisualElement<VisualElement>(LeftContainer, "header-icon");
        WindowLabel = UIShortcuts.CreateVisualElement<Label>(LeftContainer, "window-header-label");
        WindowLabel.text = "Window";
    }

    public new class UxmlFactory : UxmlFactory<WindowWithIconAndLabel> { }
}
