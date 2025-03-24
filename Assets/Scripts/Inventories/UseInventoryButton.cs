using UnityEngine;

public class UseInventoryButton : InventoryButton
{
    public void ConsumeItem()
    {
        Consumable consumable = item.GetComponent<Consumable>();
        if (consumable) inventory.ConsumeItem(consumable);
    }
}
