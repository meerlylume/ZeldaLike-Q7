using UnityEngine;

public class UseInventoryButton : InventoryButton
{
    [SerializeField] private InventoryGrid inventoryGrid; //for menu dialogue
    public void SetInventoryGrid(InventoryGrid value) { inventoryGrid = value; }
    public void UseItem()
    {
        Consumable consumable = inventory.GetInventoryData().items[itemIndex].GetComponent<Consumable>();

        consumable.TryGetComponent(out BoostCrystal boostCrystal);
        if (boostCrystal) boostCrystal.SetInventoryGrid(inventoryGrid);

        if (consumable) consumable.OnInventoryUse(inventory);
    }
}
