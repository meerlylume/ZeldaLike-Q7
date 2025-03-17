using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected InventoryData inventory;

    public virtual void Add(Item item, int quantity)
    {
        //handle stacks

        //if the item is NOT in the inventory
        //HANDLE STACKS

        //if the item is ALREADY in the inventory

        for (int i = 0; inventory.items.Count > i; i++)
        {
            if (inventory.items[i] == item)
            {
                if (inventory.quantities.Count > i) 
                {
                    //ALLER GO MANGER
                }
            }
        }
    }

    public virtual void Remove(Item item, int quantity)
    {
        //handle stacks

    }

    public bool IsInInventory(Item item)
    {
        for (int i = 0; inventory.items.Count > i; i++) { if (inventory.items[i] == item) return true; }

        return false;
    }
}
