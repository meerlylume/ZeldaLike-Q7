using UnityEngine;

public abstract class Consumable : Item
{
    public abstract void Consume(Fight user);

    public virtual void OnInventoryUse(PlayerInventory inventory) 
    { 
        Consume(inventory.GetFight());
        inventory.RemoveItem(this);
    }
}
