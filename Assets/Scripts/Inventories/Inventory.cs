using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventoryData itemLibrary;
    [SerializeField] protected InventoryData inventory;

    public virtual void AddItem(Item item, int quantity)
    {
        //don't forget to HANDLE STACKS for PLAYER ONLY

        //I SAID PLAYER ONLY. PLAYER ONLY. THIS MEANS NOT IN THIS SCRIPT. IN THE PLAYER INVENTORY SCRIPT. NOT THIS ONE. DID YOU GET THAT? ARE YOU SURE?

        for (int i = 0; inventory.items.Count > i; i++)
        {
            if (!inventory.items[i] == item) //if item ALREADY in inventory
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
    
    protected virtual Item FindInLibrary(Item searchFor)
    {
        foreach (Item item in itemLibrary.items) { if (item.GetType() == searchFor.GetType()) return item; }

        Debug.Log("ITEM NOT IN ITEM LIBRARY");
        return null;
    }

    public virtual void AddMoney(int amount)
    {
        if (amount <= 0) return;

        inventory.money += amount;
    }

    public virtual void RemoveItem(Item item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            RemoveUnit(FindInLibrary(item));
        }
    }

    public virtual void RemoveItem(Item item)
    {
        RemoveItem(item, 1); // this is an override, not recursive, don't panic
    }

    private void RemoveUnit(Item item)
    {
        //Note: the given item has already been found in the library

        if (inventory.quantities.Count != inventory.items.Count)
        {
            Debug.LogWarning("Inventory Quantities and Item Count do not match. ITEM NOT REMOVED.");
            return;
        }

        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i] != item) continue;

            if (inventory.quantities[i] <= 1)
            {
                inventory.quantities.RemoveAt(i);
                inventory.items.RemoveAt(i);
                return;
            }

            inventory.quantities[i]--;
        }
    }

    public virtual void RemoveMoney(int amount)
    {
        if (amount > 0) { return; }

        else { inventory.money -= amount; }
    }

    public bool IsInInventory(Item item)
    {
        for (int i = 0; inventory.items.Count > i; i++) { if (inventory.items[i] == item) return true; }

        return false;
    }

    public void EmptyItems()
    {
        for (int i = 0; inventory.items.Count > i; i++)
        {
            inventory.items.RemoveAt(i);
            inventory.quantities.RemoveAt(i);
        }
    }

    public void EmptyMoney()
    {
        inventory.money = 0;
    }
    public void WipeInventory() { inventory.WipeInventory(); }
}
