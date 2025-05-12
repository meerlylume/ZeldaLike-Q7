using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // PLAYER
        // Position
    public Vector2 playerPosition;

        // Unlocks
    public bool manaChargeUnlocked;

        // Stats
    public float maxHP;
    public float currentHP;

    public float maxMana;
    public float currentMana;

    public int   maxAttack;
    public int   currentAttack;

    public int   maxDefence;
    public int   currentDefence;

    public int   maxCreativity;
    public int   currentCreativity;

    public int   maxRecovery;
    public int   currentRecovery;

        // Inventory
    public int        money;
    public List<Item> items;
    public List<int>  quantities;

    // CHESTS   
    public List<ChestData> chests = new List<ChestData>();
}
