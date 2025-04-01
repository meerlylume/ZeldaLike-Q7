using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInventory : Inventory
{
    [Header("Inventory Size")]
    [SerializeField] private int maxInventorySize;
    private int inventorySize;

    [Header("References")]
    [SerializeField] PlayerFight playerFight;
    [SerializeField] EventSystem eventSystem;

    [Header("UI")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject inventoryGrid;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI descText;
    List<GameObject> slotList;

    public InventoryData GetInventoryData() { return inventory; }
    public void SetInventoryData(InventoryData value) { inventory = value; }
    public void WipeInventory()
    {
        inventory.items.Clear();
        inventory.quantities.Clear();
    }
    public override void AddItem(Item item, int quantity)
    {
        //check max size
        if (quantity <= 0) return;

        for (int i = 0; inventory.items.Count > i; i++) //Parse through the inventory
        {
            for (int j = 0; quantity > j; j++)          //Once every quantity..
            {
                if (inventory.items[i] == item)         //..if the parsed item is the same as the added item..
                {
                    //..then if it doesn't go over the maxStackQuantity, add 1 to the inventory quantity and remove 1 from the added quantity
                    if (inventory.quantities[i] + 1 <= inventory.items[i].maxStackQuantity)
                    {
                        inventory.quantities[i]++;
                        quantity--;
                    }
                }
            }
        }

        //..and at the end, if there's any quantity left or the item wasn't found in the parse inventory..
        if (quantity == 0)
        {
            RefreshInventory();
            return;
        }

        if (quantity <= item.maxStackQuantity) //..then if the remaining quantity is smaller or equal to the max stack quantity
        {
            //..add these two and be on your merry way..
            inventory.items.Add(FindInLibrary(item));
            inventory.quantities.Add(quantity);
        }

        else //..but if it's bigger instead
        {
            for (int i = 0; quantity / item.maxStackQuantity < i; i++) //..then for as many times as you can fit maxQuantities in quantity..
            {
                inventory.items.Add(FindInLibrary(item));                             //..add an item..
                if (quantity >= item.maxStackQuantity)                 //..and if the quantity is bigger or equal to its max quantity..
                {
                    inventory.quantities.Add(item.maxStackQuantity);   //..then set its quantity to the max quantity..
                    quantity -= item.maxStackQuantity;                 //..and substract quantity by maxQuantity
                }
                else inventory.quantities.Add(quantity); //..if instead quantity is smaller than maxQuantity, set its quantity to this value.
            }
        }

        RefreshInventory();
    }

    private void Start()
    {
        RefreshInventory();
    }

    public void ConsumeItem(Consumable item)
    {
        item.Consume(playerFight.GetStats());
        RemoveItem(item);
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
            GameObject newSlot          = Instantiate(slotPrefab);
            newSlot.transform.SetParent(inventoryGrid.transform, false);
            Slot newScript              = newSlot.GetComponent<Slot>();

            newScript.SetItem(inventory.items[i]);
            newScript.SetQuantity(inventory.quantities[i]);
            newScript.SetInventory(this);
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
}
