using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    [Header("Inventory Size")]
    [SerializeField] private int maxInventorySize;
    private int inventorySize;

    [Header("References")]
    [SerializeField] EventSystem eventSystem;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject inventoryGrid;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI descText;
    List<GameObject> slotList;

    /* TO DO:
     * Handle Item Descriptions
     * Handle UI Navigation
     * Take a break and have some tea
     */

    public override void AddItem(Item item, int quantity)
    {
        //check max size
        //handle stacks
        base.AddItem(item, quantity);
        RefreshInventory();
    }

    private void Start()
    {
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

    private void RefreshInventory() //later replace this with overrides
    {
        InventoryGrid_KillAllChildren();

        for (int i = 0; i < inventory.items.Count; i++)
        {
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
            Destroy(inventoryGrid.transform.GetChild(i).gameObject);
        }
    }
}
