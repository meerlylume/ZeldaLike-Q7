using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent RespawnEnemiesEvent;

    private List<EnemyFight> enemies = new List<EnemyFight>();

    public void AddEnemy(EnemyFight enemy) { enemies.Add(enemy); }

    private void Awake()
    {
        if (Instance == null)      { Instance = this;     }
        else if (Instance != null) { Destroy(gameObject); }
    }
    public void OnRespawnEnemies() { foreach (var enemy in enemies) enemy.Respawn(); }
}
