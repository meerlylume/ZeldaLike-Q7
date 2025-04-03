using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostCrystal : Consumable
{
    protected InventoryGrid inventoryGrid; //for menu dialogue

    public void SetInventoryGrid(InventoryGrid value)   { inventoryGrid      = value; }

    public override void Consume(Fight user)
    {
        Debug.LogWarning("Consume(Fight user) is irrelevant to this class and should not be called.");
    }

    public override void OnInventoryUse(PlayerInventory inventory)
    {
        if (inventoryGrid == null) Debug.Log("INVENTORY GRID NULL");
        inventoryGrid.GetPromptPanel().SetActive(true);
        inventoryGrid.GetPromptText().text = "Boost which stat?";
    }
}
