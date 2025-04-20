using UnityEngine;

public class InventoryButton : MonoBehaviour
{
    protected int itemIndex;
    protected PlayerInventory inventory;
    protected int quantity;

    public void SetItemIndex(int value)             { itemIndex = value;      }
    public void SetInventory(PlayerInventory value) { inventory = value; }
    public void SetQuantity(int value)              { quantity = value;  }
}
