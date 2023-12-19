using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUD : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDoc;

    [SerializeField]
    private StyleSheet styles;

    private VisualElement root;
    private ProgressBar healthBar;

    // Generate UI at game start
    void Start() {
        Generate();
    }

    // Refresh the UI in editor after changes have been saved
    private void OnValidate() {
        if (Application.isPlaying) return;
        Generate();
    }

    // Generate the ui
    private void Generate() {
        if (uiDoc.visualTreeAsset == null) {
            // Create new visual tree asset 
            uiDoc.visualTreeAsset = ScriptableObject.CreateInstance<VisualTreeAsset>();
        }    

        // Get root
        root = uiDoc.rootVisualElement;
        // Add our custom stylesheet to the document
        root.styleSheets.Add(styles);

        // Create healthbar
        healthBar = new ProgressBar();
        healthBar.title = "HP";
        healthBar.value = 100f;
        healthBar.AddToClassList("health-bar");
        // Add healthbar
        root.Add(healthBar);
    }

    public void SetHealth(float newHealth) {
        healthBar.value = newHealth;
    }

}
