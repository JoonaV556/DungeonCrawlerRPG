using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// Abstract class for constructing different parts of game UI. Create child classes for each part of UI, sucha as GameHUD, DebugHUD etc.
/// </summary>
public abstract class ConstructUI : MonoBehaviour
{
    [SerializeField, Tooltip("UIDocument component to construct the ui into")]
    private UIDocument uiDoc;

    [SerializeField, Tooltip("Stylesheet to use in the UI")]
    private StyleSheet styles;

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
        // Add our custom stylesheet to the root element
        RootElement.styleSheets.Add(styles);
    }

    /// <summary>
    /// Creates a visual element of specified type. Optionally adds it as a child to a specified parent & Assigns the specified classes to it
    /// </summary>
    protected VisualElement CreateVisualElement<T>(VisualElement parentElement = null, string[] classes = null) where T : VisualElement, new() {
        // Create new element of specified type
        T newElement = new T();

        // If parent is specified, add it as a child of specific parent element 
        if (parentElement != null) {
            parentElement.Add(newElement);
        }

        // Assign specified classes to the visual element
        if (classes != null) {
            foreach (string className in classes) {
                newElement.AddToClassList(className);
            }
        }

        return newElement;
    }

    /// <summary>
    /// Hides a visual element by setting its displayStyle to none
    /// </summary>
    protected void HideElement(VisualElement element) {
        element.style.display = DisplayStyle.None;
    }

    /// <summary>
    /// Enables a visual element by setting its display to flex
    /// </summary>
    protected void ShowElement(VisualElement element) {
        element.style.display = DisplayStyle.Flex;
    }
}
