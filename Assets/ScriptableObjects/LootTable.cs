using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loot", menuName = "Scriptable Objects/Loot")]
public class LootTable : InventoryData
{
    public List<float> odds;
}
