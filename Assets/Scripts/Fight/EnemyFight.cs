using System.Collections;
using UnityEngine;

public class EnemyFight : Fight
{
    protected Stats baseStats;
    [Header("Monster Loot")]
    [SerializeField] protected LootTable lootTable;
    [SerializeField] protected float lootRadius;
    [SerializeField] protected float timeBetweenDrops = 0.1f;
    protected Vector3 deathPos;
    protected Vector3 spawnPos;
    public static int enemyKillCount;

    public override void Start()
    {
        spawnPos  = transform.position;
        baseStats = stats;
        stats     = ScriptableObject.CreateInstance<Stats>();
        healthBar = healthBarObject.GetComponent<GUIStatBar>();
        CopyBaseStats();

        EventManager.Instance.AddEnemy(this);

        base.Start();
    }
    private void CopyBaseStats() { CopyStatsInto(baseStats, stats); }

    public override void Die(Stats killer)
    {
        canTakeDamage = false;
        enemyKillCount++;

        StartCoroutine(DeathRoutine(killer));
    }

    public void DropItem(Item item) 
    {
        //Made this into a seperate function so I can reuse it, for example for a mimic enemy that drops money when it is damaged

        Item newItem = Instantiate(item);

        newItem.transform.position = new Vector3(Random.Range(transform.position.x - lootRadius, transform.position.x + lootRadius),
                                                 Random.Range(transform.position.y - lootRadius, transform.position.y + lootRadius), 0);
    }

    IEnumerator DeathRoutine(Stats killer)
    {
        if (!lootTable) yield break;

        //FAILSAFE: 
            //If quantities or odds list is wrong, drop every item once regardless of rarity
        if (!((lootTable.items.Count == lootTable.quantities.Count) == (lootTable.items.Count == lootTable.odds.Count)))
        {
            Debug.LogError("Wrong LootTable. Dropping every item once.");
            for (int i = 0; lootTable.items.Count > 0; i++)
            {
                DropItem(lootTable.items[i]);
                yield return new WaitForSeconds(timeBetweenDrops);
            }
        }

        //Regular behaviour
        else
        {
            for (int i = 0; lootTable.items.Count > i; i++)
            {
                for (int j = 0; lootTable.quantities[i] > j; j++)
                {
                    //Find a random between 0 and 1 - LootModifier, if it is smaller than this item's dropping odds, drop it.
                    if (Random.Range(0.0f, 1.0f - killer.LootModifier()) <= lootTable.odds[i])
                    {
                        // If Loot Modifier is maxxed out, player has a 50% chance of dropping twice the loot
                        if (killer.IsLootMaxxedOut() && Random.Range(0.0f, 1.0f) >= 0.5f) DropItem(lootTable.items[i]);
                        
                        DropItem(lootTable.items[i]);
                        yield return new WaitForSeconds(timeBetweenDrops);
                    }
                }
            }
        }

        Die();
        healthBarObject.SetActive(isAlive);
    }

    public override void RefreshMana()
    {
        // Enemies have no mana bar, and don't use mana at all, at least for now
        return;
    }

    public virtual void Respawn()
    {
        isAlive                 = true;

        healthBarObject.SetActive(true);
        canTakeDamage           = true;
        collider2d.enabled      = true;
        spriteRenderer.enabled  = true;
        rb.linearVelocity       = Vector2.zero;
        transform.position      = spawnPos;

        FullHeal();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lootRadius);
    }
}
