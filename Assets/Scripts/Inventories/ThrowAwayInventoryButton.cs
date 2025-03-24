using UnityEngine;

public class ThrowAwayInventoryButton : InventoryButton
{
    public void ThrowAwayItem()
    {
        inventory.RemoveItem(item, quantity);
        inventory.RefreshInventory();
    }
}
