using UnityEngine;

public class IC_PhysicalBoostCrystal : BoostCrystal
{
    [Header("Boost Amount")]
    public int boostAmount;

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
    }

    public void RaiseAttack()
    {
        playerInventory.GetFight().RaiseAttack(boostAmount);
        EndChoose();
    }

    public void RaiseDefence()
    {
        playerInventory.GetFight().RaiseDefence(boostAmount);
        EndChoose();
    }


    public override void EndChoose()
    {
        base.EndChoose();

        playerInventory.RemoveItem(this);

        if (attackButton) Destroy(attackButton.gameObject);
        if (defenceButton) Destroy(defenceButton.gameObject);
    }
}
