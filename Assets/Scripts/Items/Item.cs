using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [Header("Appearance")]
    public new string name    = "Item";
    public string description = "Placeholder";
    public Sprite sprite;
    [Space]

    [Header("Properties")]
    public int maxStackQuantity = 64;
    public int basePrice = 0;
    public bool canBeSold = true;
}
