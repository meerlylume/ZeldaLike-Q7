using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoostCrystal : Consumable
{
    protected InventoryGrid inventoryGrid; //for menu dialogue
    protected MenuOption cancelButton;

    public void SetInventoryGrid(InventoryGrid value)   { inventoryGrid      = value; }

    public override void Consume(Fight user)
    {
        Debug.LogWarning("Consume(Fight user) is irrelevant to this class and should not be called.");
    }

    public void Cancel() { EndChoose(); }

    public void ShowPrompt()
    {
        if (inventoryGrid == null) Debug.LogError("Inventory Grid Null");
        inventoryGrid.GetPromptPanel().SetActive(true);
        inventoryGrid.GetPromptText().text = "Boost which stat?";
        inventoryGrid.GetChoicesGrid().SetActive(true);
    }

    protected GameObject CreateButton()
    {
        GameObject newBtn = Instantiate(inventoryGrid.GetChoicePrefab());
        newBtn.transform.SetParent(inventoryGrid.GetChoicesGrid().transform);
        newBtn.transform.localScale = Vector3.one;

        return newBtn;
    }

    public virtual void EndChoose()
    {
        inventoryGrid.GetChoicesGrid()?.SetActive(false);
        inventoryGrid.GetPromptPanel()?.SetActive(false);
        if (cancelButton) Destroy(cancelButton.gameObject);
    }
}
