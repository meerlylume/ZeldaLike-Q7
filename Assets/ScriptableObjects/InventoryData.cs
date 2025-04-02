using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Scriptable Objects/Inventory")]
public class InventoryData : ScriptableObject
{
    [Header("Identifier")]
    public int money; 
    [Space]


    [Header("Inventory")]
    //I am so pissed that dictionaries aren't serializable istg
    public List<Item> items;
    public List<int> quantities;

    public void WipeInventory()
    {
        items.Clear();
        quantities.Clear();
        money = 0;
    }
}
