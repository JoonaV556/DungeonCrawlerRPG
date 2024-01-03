using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Abstract class for constructing different parts of game UI. Create child classes for each part of UI, sucha as GameHUD, DebugHUD etc.
/// </summary>
public abstract class ConstructUI : MonoBehaviour
{
    [Header("General - REQUIRED")]
    [SerializeField, Tooltip("UIDocument component to construct the ui into")]
    private UIDocument uiDoc;

    [SerializeField, Tooltip("Stylesheet to use in the UI")]
    private StyleSheet styles;

    [SerializeField, Tooltip("Should the UI be displayed in the Game window outside of Play mode?")]
    private bool EnableEditorPreview = false;

    protected VisualElement RootElement; // Root element of the created UXML tree

    // Generate UI at game start
    private void Awake() {
        // Do the initial construction
        PreConstruct();
        // Do the user defined construction
        Construct();
    }

    // Refresh the UI in editor after changes have been saved

    private void OnValidate() {
        if (Application.isPlaying) return;
        if (!EnableEditorPreview) return;

        // Do the initial construction
        PreConstruct();
        // Do the user defined construction
        Construct();
    }

    /// <summary>
    /// Construct the UI elements in this method
    /// </summary>
    protected abstract void Construct();

    /// <summary>
    /// Does the initial UI construction steps before the child defined construction steps
    /// </summary>
    private void PreConstruct() {
        if (uiDoc == null) return;

        // Create new visual tree asset 
        uiDoc.visualTreeAsset = ScriptableObject.CreateInstance<VisualTreeAsset>();
        // Get the root element
        RootElement = uiDoc.rootVisualElement;

        if (styles == null) { return; }
        // Add our custom stylesheet to the root element
        RootElement.styleSheets.Add(styles);
    }
}
