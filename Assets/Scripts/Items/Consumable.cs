using UnityEngine;

public abstract class Consumable : Item
{
    public abstract void Consume(Fight user);

    public virtual void UseItem(Inventory inventory) { inventory.ConsumeItem(this); }
}
