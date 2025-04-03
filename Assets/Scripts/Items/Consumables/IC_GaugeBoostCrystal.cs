using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IC_GaugeBoostCrystal : BoostCrystal
{
    [Header("HP/Mana Boost Ranges")]
    public int minRandomRange = 5;
    public int maxRandomRange = 10;

    protected PlayerInventory playerInventory;


    public override void OnInventoryUse(PlayerInventory inventory)
    {
        playerInventory = inventory;

        //Prompt which stat to use
        base.OnInventoryUse(inventory);

        // Handle HP choice
        GameObject hpOption = Instantiate(inventoryGrid.GetChoicePrefab());
        hpOption.transform.SetParent(inventoryGrid.GetChoicesGrid().transform);
        hpOption.transform.localScale = Vector3.one;
        hpOption.TryGetComponent(out MenuOption hpButton);
        hpButton.SetText("HP");
        hpButton.Submit.AddListener(RaiseMaxHP);

        // Handle Mana choice
        GameObject manaOption = Instantiate(inventoryGrid.GetChoicePrefab());
        manaOption.transform.SetParent(inventoryGrid.GetChoicesGrid().transform);
        manaOption.transform.localScale = Vector3.one;
        manaOption.TryGetComponent(out MenuOption manaButton);
        manaButton.SetText("Mana");
        manaButton.Submit.AddListener(RaiseMaxMana);
    }

    public void RaiseMaxHP()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseMaxHP(amount);
        playerInventory.RemoveItem(this);
        inventoryGrid.GetPromptPanel().SetActive(false);
    }

    public void RaiseMaxMana()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseMaxMana(amount);
        playerInventory.RemoveItem(this);
        inventoryGrid.GetPromptPanel().SetActive(false);
    }
}

