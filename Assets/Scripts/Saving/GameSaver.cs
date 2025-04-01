using System.Collections.Generic;
using System.IO;
using UnityEditor.Overlays;
using UnityEngine;

public class GameSaver : MonoBehaviour
{
    private string saveLocation;
    [Header("Defaults")]
    [SerializeField] private Stats cannelleFirstStats;

    private void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        LoadGame();
    }

    public void SaveGame()
    {
        GameObject player  = GameObject.FindGameObjectWithTag("Player");

        SaveData saveData  = new()
        {
            playerPosition = player.transform.position,
            playerStats    = player.GetComponent<PlayerFight>().GetStats(),
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

            // Chest
            Chest[] chests = FindObjectsByType<Chest>(FindObjectsSortMode.None);
            foreach (Chest chest in chests) 
            {
                Debug.Log(saveData.chests.Count);

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
        SaveData saveData    = new() { };
        saveData.playerStats = CopyStats(cannelleFirstStats, saveData.playerStats);

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
        Debug.Log("OnApplicationQuit()");
    }

    private Stats CopyStats(Stats from, Stats to)
    {
        to.name             = from.name;
        to.isAlly           = from.isAlly;
        to.maxHP            = from.maxHP;
        to.currentHP        = from.currentHP;
        to.maxMana          = from.maxMana;
        to.currentMana      = from.currentMana;
        to.attack           = from.attack;
        to.defence          = from.defence;
        to.creativity       = from.creativity;
        to.recovery         = from.recovery;
        to.movementSpeed    = from.movementSpeed;
        to.cooldownModifier = from.cooldownModifier;

        return to;
    }

    private void SaveChests(SaveData saveData)
    {
        Debug.Log("SaveChests()");

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