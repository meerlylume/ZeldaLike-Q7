using UnityEngine;

public class UseInventoryButton : MonoBehaviour
{
    private Item item;
    private Inventory inventory;

    public void SetItem(Item value)           { item = value;      }
    public void SetInventory(Inventory value) { inventory = value; }

    public void ConsumeItem()
    {
        Consumable consumable = item.GetComponent<Consumable>();
        if (consumable) inventory.ConsumeItem(consumable);
    }
}
