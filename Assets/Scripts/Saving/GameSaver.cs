using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private string saveLocation;
    [Header("Defaults")]
    [SerializeField] private Stats cannelleFirstStats;
    [SerializeField] private Stats cannelleCurrentStats;
    [SerializeField] private InventoryData cannelleInventoryData;

    private void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        GameObject player   = GameObject.FindGameObjectWithTag("Player");

        SaveData saveData   = new()
        {
            playerPosition  = player.transform.position,
            playerStats     = player.GetComponent<PlayerFight>().GetStats(),
            playerInventory = player.gameObject.GetComponent<PlayerInventory>().GetInventoryData(),
        };

        SaveChests(saveData);

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            // Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = saveData.playerPosition;
            player.GetComponent<PlayerFight>().SetStats(saveData.playerStats);
            player.GetComponent<PlayerInventory>().SetInventoryData(saveData.playerInventory);

            // Chest
            Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest chest in chests) 
            {
                for (int i = 0; saveData.chests.Count > i; i++)
                {
                    if (!(saveData.chests[i].ID == chest.GetID())) continue;

                    if (saveData.chests[i].IsOpen) { chest.Interact(); }
                }
            }
        }
        else { SaveGame(); }
    }

    public void DeleteSave()
    {
        SaveData saveData = new()
        {
            playerInventory = cannelleInventoryData
        };

        // Player Stats
        CopyStats(cannelleFirstStats, cannelleCurrentStats);
        saveData.playerStats = cannelleCurrentStats;
        // Inventory
        if (saveData.playerInventory) saveData.playerInventory.WipeInventory();
        else
        {
            GameObject player        = GameObject.FindGameObjectWithTag("Player");
            saveData.playerInventory = player.GetComponent<PlayerInventory>().GetInventoryData();
            saveData.playerInventory.WipeInventory();
        }

        // Write
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }

    private void CopyStats(Stats from, Stats to)
    {
        to.isAlly           = from.isAlly;
        to.name             = from.name;

        to.maxHP            = from.maxHP;
        to.currentHP        = from.currentHP;
        to.maxMana          = from.maxMana;
        to.currentMana      = from.currentMana;

        to.currentATK           = from.currentATK;
        to.currentDEF          = from.currentDEF;
        to.currentCRE       = from.currentCRE;
        to.currentRCV         = from.currentRCV;

        to.movementSpeed    = from.movementSpeed;
        to.attackCooldownModifier = from.attackCooldownModifier;
    }

    private void SaveChests(SaveData saveData)
    {
        saveData.chests = new List<ChestData>();
        Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);

        foreach (Chest chest in chests) 
        { 
            ChestData newChest = new ChestData();
            newChest.ID = chest.GetID();
            newChest.IsOpen = chest.GetIsOpen();
            saveData.chests.Add(newChest);
        }
    }
}