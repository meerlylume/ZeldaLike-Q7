using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] protected InventoryData inventory;

    public virtual void Add(Item item, int quantity)
    {
        //HANDLE STACKS for PLAYER ONLY

        for (int i = 0; inventory.items.Count > i; i++)
        {
            if (inventory.items[i] == item) //if item ALREADY in inventory
            {
                if (inventory.quantities.Count - 1 >= i)  //if I didn't fuck up in the inspector
                {
                    inventory.quantities[i] += quantity;
                    return;
                }
            }
        }

        //if item NOT in inventory
        inventory.items.Add(item);
        inventory.quantities.Add(quantity);
    }

    public virtual void Remove(Item item, int quantity)
    {
        //handle stacks

        for (int i = 0; inventory.items.Count > i; i++)
        {
            if (inventory.items[i] == item) //if item in inventory
            {
                if (inventory.quantities.Count - 1 >= i)  //if I didn't fuck up in the inspector
                {
                    inventory.quantities[i] -= quantity;
                    if (inventory.quantities[i] <= 0)
                    {
                        inventory.quantities.RemoveAt(i);
                        inventory.items.RemoveAt(i);
                    }
                    return;
                }
            }

            
        }

         //if item NOT in inventory
        Debug.Log("ITEM NOT IN INVENTORY");
    }

    public bool IsInInventory(Item item)
    {
        for (int i = 0; inventory.items.Count > i; i++) { if (inventory.items[i] == item) return true; }

        return false;
    }

    public void EmptyInventory()
    {
        for (int i = 0; inventory.items.Count > i; i++)
        {
            inventory.items.RemoveAt(i);
            inventory.quantities.RemoveAt(i);
        }
    }
}
