using UnityEngine;

public class IC_SpiritualBoostCrystal : BoostCrystal
{
    [Header("Creativity/Recovery Boost Ranges")]
    public int boostAmount;

    protected PlayerInventory playerInventory;
    MenuOption creativityButton;
    MenuOption recoveryButton;

    public override void OnInventoryUse(PlayerInventory inventory)
    {
        playerInventory = inventory;

        //Prompt which stat to use
        ShowPrompt();

        // Handle creativity choice
        GameObject creativityOption = CreateButton();
        creativityOption.TryGetComponent(out creativityButton);
        creativityButton.SetText("Creativity");
        creativityButton.Submit.AddListener(RaiseCreativity);

        // Handle recovery choice
        GameObject recoveryOption = CreateButton();
        recoveryOption.TryGetComponent(out recoveryButton);
        recoveryButton.SetText("Recovery");
        recoveryButton.Submit.AddListener(RaiseRecovery);
    }

    public void RaiseCreativity()
    {
        playerInventory.GetFight().RaiseCreativity(boostAmount);
        EndChoose();
    }

    public void RaiseRecovery()
    {
        playerInventory.GetFight().RaiseRecovery(boostAmount);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        playerInventory.RemoveItem(this);

        if (creativityButton) Destroy(creativityButton.gameObject);
        if (recoveryButton) Destroy(recoveryButton.gameObject);
    }
}
