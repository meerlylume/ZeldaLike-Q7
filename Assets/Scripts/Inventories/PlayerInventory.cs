using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [Header("Inventory Size")]
    [SerializeField] private int maxInventorySize;
    private int inventorySize;

    [Header("References")]
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject inventoryGrid;
    List<GameObject> slotList;

    public override void Add(Item item, int quantity)
    {
        //check max size
        //handle stacks
        base.Add(item, quantity);
        RefreshInventory();
    }

    private void Start()
    {
        RefreshInventory();
    }

    private void RefreshInventory() //later replace this with overrides
    {
        InventoryGrid_KillAllChildren();

        for (int i = 0; i < inventory.items.Count; i++)
        {
            Debug.Log(inventory.items[i].name + i);
            GameObject newSlot          = Instantiate(slotPrefab);
            newSlot.transform.SetParent(inventoryGrid.transform, false);
            Slot newScript              = newSlot.GetComponent<Slot>();

            newScript.quantityText.text = "x" + inventory.quantities[i];
            newScript.image.sprite      = inventory.items[i].sprite;
        }
    }

    private void InventoryGrid_KillAllChildren() //i love this function name so much
    {
        for (int i = 0; i < inventoryGrid.transform.childCount; i++)
        {
            //for (int j = 0; j < inventoryGrid.transform.GetChild(i).childCount; j++)
            //{
            //    Destroy(inventoryGrid.transform.GetChild(j).gameObject);
            //}
            Destroy(inventoryGrid.transform.GetChild(i).gameObject);
        }
    }
}
