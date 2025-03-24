using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    protected Item item;
    protected PlayerInventory inventory;
    protected int quantity;

    public void SetItem(Item value)                 { item = value;      }
    public void SetInventory(PlayerInventory value) { inventory = value; }
    public void SetQuantity(int value)              { quantity = value;  }
}
