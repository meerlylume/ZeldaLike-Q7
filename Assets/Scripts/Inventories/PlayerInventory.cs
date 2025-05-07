using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventory : Inventory
{
    [Header("Inventory Size")]
    [SerializeField] private int maxInventorySize;

    [Header("References")]
    [SerializeField] PlayerFight playerFight;
    [SerializeField] EventSystem eventSystem;

    [Header("UI")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject inventoryGrid;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI descText;
    List<GameObject> slotList;

    #region Get/Set
    public InventoryData GetInventoryData()           { return inventory;   }
    public Fight GetFight()                           { return playerFight; }
    public void SetInventoryData(InventoryData value) { inventory = value;  }
    #endregion

    private void Start() { RefreshInventory(); }

    public override void AddItem(Item item, int quantity)
    {
        if (quantity <= 0) return;

        for (int i = 0; i < quantity; i++) { AddUnit(FindInLibrary(item)); }
        
        RefreshInventory();
    }

    private void AddUnit(Item item)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            // Check if items match
            if (FindInLibrary(inventory.items[i]) != item) continue;

            // Check if found item match's quantity is smaller than that item's max quantity
            if (inventory.quantities[i] >= item.maxStackQuantity) { continue; }
            else 
            { 
                inventory.quantities[i]++;
                return;
            }
        }

        if (inventory.items.Count >= maxInventorySize)
        {
            Debug.Log("Inventory maximum size (" + maxInventorySize + ") reached, but there is no implementation for this case yet. Added item is void.");
            return;
        }

        inventory.items.Add(item);
        inventory.quantities.Add(1);
    }

    public override void RemoveItem(Item item, int quantity)
    {
        base.RemoveItem(item, quantity);
        RefreshInventory();
    }

    public void OnInventoryOpen()
    {
        if (slotList != null)
        {
            eventSystem.SetSelectedGameObject(slotList[0]);
            //Button button = slotList[0].GetComponent<Button>();
        }
    }

    public void RefreshInventory()
    {
        //This is only temporary, killing all children everytime is a bit- well, overkill.

        InventoryGrid_KillAllChildren();

        for (int i = 0; i < inventory.items.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab);
            newSlot.transform.SetParent(inventoryGrid.transform, false);
            Slot newScript     = newSlot.GetComponent<Slot>();

            newScript.SetItemIndex(i);
            newScript.SetInventory(this);
            newScript.SetInventoryData(inventory);
            newScript.RefreshSlot();
        }

        OnInventoryOpen();
    }

    private void InventoryGrid_KillAllChildren() //i love this function name so much
    {
        for (int i = 0; i < inventoryGrid.transform.childCount; i++)
        {
            Destroy(inventoryGrid.transform.GetChild(i).gameObject);
        }
    }

    public bool CheckIfInInventory(Item item, int quantity)
    {
        for (int i = 0; i < inventory.items.Count; i++)
            if (inventory.items[i] == item && inventory.quantities[i] >= quantity) return true;

        return false;
    }

    public bool CheckIfInInventory(Item item)
    {
        for (int i = 0; i < inventory.items.Count; i++) if (inventory.items[i] == item) return true;

        return false;
    }
}
