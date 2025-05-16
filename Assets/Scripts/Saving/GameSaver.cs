using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaver : MonoBehaviour
{
    public static GameSaver Instance;

    private string saveLocation;
    [Header("Defaults")]
    [SerializeField] private Stats cannelleFirstStats;
    [SerializeField] private Stats cannelleCurrentStats;
    [SerializeField] private InventoryData cannelleInventoryData;

    private void Awake()
    {
        if (Instance == null)      { Instance = this;     }
        else if (Instance != null) { Destroy(gameObject); }

        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveGame()
    {
        // Get Player in scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Get Player's stats
        PlayerFight playerFight = player.GetComponent<PlayerFight>();
        Stats pStats = playerFight.GetStats();

            // Get Player's inventory
        PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
        InventoryData inventoryData = playerInventory.GetInventoryData();

        SaveData saveData = new()
        {
            isDefaultSave = false,

            //player pos
            playerPosition = player.transform.position,
            //unlocks
            manaChargeUnlocked = playerFight.GetCanChargeMana(),
            //stats
            maxHP              = pStats.maxHP,
            currentHP          = pStats.currentHP,
            maxMana            = pStats.maxMana,
            currentMana        = pStats.currentMana,
            maxAttack          = pStats.attack,
            currentAttack      = pStats.currentATK,
            maxDefence         = pStats.defence,
            currentDefence     = pStats.currentDEF,
            maxCreativity      = pStats.creativity,
            currentCreativity  = pStats.currentCRE,
            maxRecovery        = pStats.recovery,
            currentRecovery    = pStats.currentRCV,
            //inventory
            money      = inventoryData.money,
            items      = inventoryData.items,
            quantities = inventoryData.quantities,
        };

        SaveChests(saveData);

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (SceneManager.GetActiveScene().name != "Game")
        {
            StartCoroutine(WaitForSceneLoad());
            return;
        }

        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            // Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (!player) Debug.LogWarning("NO PLAYER GAMEOBJECT FOUND ON LOAD");
            player.transform.position = saveData.playerPosition; //position

            PlayerFight playerFight = player.GetComponent<PlayerFight>();
            playerFight.LoadStats(saveData);

            playerFight.SetCanChargeMana(saveData.manaChargeUnlocked);

            // Player Inventory
            PlayerInventory playerInventory = player.GetComponent<PlayerInventory>();
            InventoryData inventoryData     = playerInventory.GetInventoryData();
            inventoryData.money      = saveData.money;
            inventoryData.items      = saveData.items;
            inventoryData.quantities = saveData.quantities;

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
        else SaveGame();
    }

    public IEnumerator WaitForSceneLoad()
    {
        SceneManager.LoadScene("Game");

        while (!GameObject.FindGameObjectWithTag("Player")){
            yield return new WaitForEndOfFrame();
        }

        LoadGame();
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Game");
        LoadGame();
    }

    public void DeleteSave()
    {
        CopyStats(cannelleFirstStats, cannelleCurrentStats);

        SaveData saveData = new()
        {
            isDefaultSave = true,

            // Unlocks
            manaChargeUnlocked = false,

            // Stats
            maxHP = cannelleFirstStats.maxHP,
            currentHP = cannelleFirstStats.currentHP,

            maxMana = cannelleFirstStats.maxMana,
            currentMana = cannelleFirstStats.currentMana,

            maxAttack = cannelleFirstStats.attack,
            currentAttack = cannelleFirstStats.currentATK,

            maxDefence = cannelleFirstStats.defence,
            currentDefence = cannelleFirstStats.currentDEF,

            maxCreativity = cannelleFirstStats.creativity,
            currentCreativity = cannelleFirstStats.currentCRE,

            maxRecovery = cannelleFirstStats.recovery,
            currentRecovery = cannelleFirstStats.currentRCV,

            // Inventory
            money = 0,
            items = new List<Item>(),
            quantities = new List<int>(),

            //pos
            playerPosition = new Vector3(-5, 0, 0),
        };

        // Write
        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private void CopyStats(Stats from, Stats to)
    {
        to.isAlly        = from.isAlly;
        to.name          = from.name;

        to.maxHP         = from.maxHP;
        to.currentHP     = from.currentHP;

        to.maxMana       = from.maxMana;
        to.currentMana   = from.currentMana;

        to.attack        = from.attack;
        to.currentATK    = from.currentATK;

        to.defence       = from.defence;
        to.currentDEF    = from.currentDEF;

        to.creativity    = from.creativity;
        to.currentCRE    = from.currentCRE;

        to.recovery      = from.recovery;
        to.currentRCV    = from.currentRCV;

        to.movementSpeed = from.movementSpeed;
        to.attackCooldownModifier = from.attackCooldownModifier;
    }

    private void SaveChests(SaveData saveData)
    {
        saveData.chests = new List<ChestData>();
        Chest[] chests  = FindObjectsByType<Chest>(FindObjectsSortMode.None);

        foreach (Chest chest in chests) 
        { 
            ChestData newChest = new ChestData();
            newChest.ID        = chest.GetID();
            newChest.IsOpen    = chest.GetIsOpen();
            saveData.chests.Add(newChest);
        }
    }

    public bool HasSaveFile()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            return !saveData.isDefaultSave;
        }

        return false;
    }
}