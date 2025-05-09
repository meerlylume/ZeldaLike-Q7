using UnityEngine;

public class Q_Give5Carrots : Quest
{
    [Header("Item References")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private PlayerFight     playerFight;
    [SerializeField] private InventoryData   requiredItems;

    public override void CheckIfCompleted()
    {
        if (!playerInventory) return;

        for (int i = 0; i < requiredItems.items.Count; i++)
            if (!(playerInventory.CheckIfInInventory(requiredItems.items[i], requiredItems.quantities[i]))) 
                return;

        Debug.Log("Complete Quest");
        CompleteQuest();
    }

    public override void CompleteQuest()
    {
        base.CompleteQuest();

        for (int i = 0; i < requiredItems.items.Count; i++)
            playerInventory.RemoveItem(requiredItems.items[i], requiredItems.quantities[i]);

        playerFight.SetCanChargeMana(true);
    }
}
