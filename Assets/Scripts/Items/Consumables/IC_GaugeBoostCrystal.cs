using TMPro;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;

public class IC_GaugeBoostCrystal : BoostCrystal
{
    [Header("HP/Mana Boost Ranges")]
    public int minRandomRange = 5;
    public int maxRandomRange = 10;

    protected PlayerInventory playerInventory;
    MenuOption hpButton;
    MenuOption manaButton;

    public override void OnInventoryUse(PlayerInventory inventory)
    {
        playerInventory = inventory;

        //Prompt which stat to use
        ShowPrompt();

        // Handle HP choice
        GameObject hpOption = CreateButton();
        hpOption.TryGetComponent(out hpButton);
        hpButton.SetText("HP");
        hpButton.Submit.AddListener(RaiseMaxHP);

        // Handle Mana choice
        GameObject manaOption = CreateButton();
        manaOption.TryGetComponent(out manaButton);
        manaButton.SetText("Mana");
        manaButton.Submit.AddListener(RaiseMaxMana);

        // Handle Cancel
        GameObject cancelOption = CreateButton();
        cancelOption.TryGetComponent(out cancelButton);
        cancelButton.SetText("Cancel");
        cancelButton.Submit.AddListener(Cancel);
    }

    public void RaiseMaxHP()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseMaxHP(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }

    public void RaiseMaxMana()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseMaxMana(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        if (hpButton)     Destroy(hpButton.gameObject);
        if (manaButton)   Destroy(manaButton.gameObject);
    }
}

