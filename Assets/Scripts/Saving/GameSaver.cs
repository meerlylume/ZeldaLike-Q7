using System.Collections.Generic;
using System.IO;
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
        GameObject player   = GameObject.FindGameObjectWithTag("Player");

        SaveData saveData   = new()
        {
            playerPosition  = player.transform.position,
            playerStats     = player.GetComponent<PlayerFight>().GetStats(),
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = saveData.playerPosition;
            player.GetComponent<PlayerFight>().SetStats(saveData.playerStats);
        }
        else
        {
            SaveGame();
        }
    }

    public void DeleteSave()
    {
        SaveData saveData  = new()
        {
            playerStats    = cannelleFirstStats,
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }
}
