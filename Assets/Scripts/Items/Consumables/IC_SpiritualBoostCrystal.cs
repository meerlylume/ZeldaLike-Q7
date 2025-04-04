using UnityEngine;

public class IC_SpiritualBoostCrystal : BoostCrystal
{
    [Header("Creativity/Recovery Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;
    /*
    public override void Consume(Fight user)
    {
        //Prompt Attack or Defence

        int amount = Random.Range(minRandomRange, maxRandomRange);
        user.RaiseCreativity(amount);
        //user.RaiseRecovery(amount);
        Debug.Log("Incomplete Implementation: SpiritualBoostCrystal only raises Creativity");
    }*/

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

        // Handle Cancel
        GameObject cancelOption = CreateButton();
        cancelOption.TryGetComponent(out cancelButton);
        cancelButton.SetText("Cancel");
        cancelButton.Submit.AddListener(Cancel);
    }

    public void RaiseCreativity()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseCreativity(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }

    public void RaiseRecovery()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseRecovery(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        if (creativityButton) Destroy(creativityButton.gameObject);
        if (recoveryButton) Destroy(recoveryButton.gameObject);
    }
}
