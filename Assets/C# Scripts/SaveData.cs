using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Player
    public Vector2 playerPosition;
    public Stats   playerStats;
    public InventoryData playerInventory;

    // Chests
    public List<ChestData> chests = new List<ChestData>();
}
