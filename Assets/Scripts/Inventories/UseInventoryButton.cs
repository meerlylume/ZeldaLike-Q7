using UnityEngine;

public class UseInventoryButton : InventoryButton
{
    public void UseItem()
    {
        Consumable consumable = inventory.GetInventoryData().items[itemIndex].GetComponent<Consumable>();
        if (consumable) consumable.UseItem(inventory);
    }
}
