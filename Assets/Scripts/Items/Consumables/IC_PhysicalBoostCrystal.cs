using UnityEngine;

public class IC_PhysicalBoostCrystal : BoostCrystal
{
    [Header("Strength/Defence Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;

    protected PlayerInventory playerInventory;
    MenuOption attackButton;
    MenuOption defenceButton;

    public override void OnInventoryUse(PlayerInventory inventory)
    {
        playerInventory = inventory;

        //Prompt which stat to use
        ShowPrompt();

        // Handle HP choice
        GameObject attackOption = CreateButton();
        attackOption.TryGetComponent(out attackButton);
        attackButton.SetText("Attack");
        attackButton.Submit.AddListener(RaiseAttack);

        // Handle Mana choice
        GameObject defenceOption = CreateButton();
        defenceOption.TryGetComponent(out defenceButton);
        defenceButton.SetText("Defence");
        defenceButton.Submit.AddListener(RaiseDefence);

        // Handle Cancel
        GameObject cancelOption = CreateButton();
        cancelOption.TryGetComponent(out cancelButton);
        cancelButton.SetText("Cancel");
        cancelButton.Submit.AddListener(Cancel);
    }

    public void RaiseAttack()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseAttack(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }

    public void RaiseDefence()
    {
        int amount = Random.Range(minRandomRange, maxRandomRange);
        playerInventory.GetFight().RaiseDefence(amount);
        playerInventory.RemoveItem(this);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        if (attackButton) Destroy(attackButton.gameObject);
        if (defenceButton) Destroy(defenceButton.gameObject);
    }
}
