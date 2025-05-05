using TMPro;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UI;

public class IC_GaugeBoostCrystal : BoostCrystal
{
    [Header("HP/Mana Boost Ranges")]
    public int boostAmount;

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
        playerInventory.GetFight().RaiseMaxHP(boostAmount);
        EndChoose();
    }

    public void RaiseMaxMana()
    {
        playerInventory.GetFight().RaiseMaxMana(boostAmount);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        playerInventory.RemoveItem(this);

        if (hpButton)     Destroy(hpButton.gameObject);
        if (manaButton)   Destroy(manaButton.gameObject);
    }
}

