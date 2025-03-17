using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] private int maxInventorySize;
    private int inventorySize;

    public override void Add(Item item, int quantity)
    {
        //check max size
        //handle stacks
        base.Add(item, quantity);
    }
}
