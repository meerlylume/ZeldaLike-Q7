using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Appearance")]
    public new string name;
    public string description;
    public Sprite sprite;

    [Header("Properties")]
    public int maxStackQuantity;
    public int basePrice;
    public bool isConsumable;
}
