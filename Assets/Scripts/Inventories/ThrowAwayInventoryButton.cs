using UnityEngine;

public class ThrowAwayInventoryButton : InventoryButton
{
    public void ThrowAwayItem()
    {
        inventory.RemoveItem(inventory.GetInventoryData().items[itemIndex], quantity);
        inventory.RefreshInventory();
    }
}
