using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class InventoryData : ScriptableObject
{
    public new string name; //Player, Chest, Shop
    public int money;

    //I am so pissed that dictionaries aren't serializable istg
    public List<Item> items;
    public List<int> quantities;
}
