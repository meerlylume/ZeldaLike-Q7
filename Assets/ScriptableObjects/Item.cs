using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    [Header("Appearance")]
    public new string name;
    public string description;
    public Sprite sprite;
    [Space]

    [Header("Properties")]
    public int maxStackQuantity = 1;
    public int basePrice;
    public bool isConsumable;
    public bool canBeSold;
    //public bool IsUsable;
}
