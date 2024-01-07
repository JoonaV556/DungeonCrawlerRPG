using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds base data for all items. Not meant to be used directly in item management. Use InventoryItem for item management.
/// </summary>
public class Item : ScriptableObject
{
    public Texture2D Icon = null;
    public string Name = "defaultName";
    public string Description = "defaultDesc";
    public bool Stackable = false;
    public int MaxStackSize = 1;
    public float Weight = 0f;
}
