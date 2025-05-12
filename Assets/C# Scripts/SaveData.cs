using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Player
    public Vector2 playerPosition;
    public Stats   playerStats;
    public InventoryData playerInventory;
    public bool manaChargeUnlocked;

    public float maxHP;
    public float maxMana;
    public int   maxAttack;
    public int   maxDefence;
    public int   maxCreativity;
    public int   maxRecovery;

    // Chests
    public List<ChestData> chests = new List<ChestData>();
}
