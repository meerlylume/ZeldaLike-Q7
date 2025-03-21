using System.Collections;
using System.Data;
using UnityEngine;

public class EnemyFight : Fight
{
    protected Stats baseStats;
    [Header("Monster Loot")]
    [SerializeField] LootTable lootTable;
    [SerializeField] float lootRadius;
    [SerializeField] float timeBetweenDrops = 0.1f;
    private Vector3 deathPos;

    public override void Start()
    {
        baseStats = stats;
        stats     = ScriptableObject.CreateInstance<Stats>();
        CopyBaseStats();

        base.Start();
    }
    private void CopyBaseStats()
    {
        stats.name             = baseStats.name;
        stats.maxHP            = baseStats.maxHP;
        stats.maxMana          = baseStats.maxMana;
        stats.attack           = baseStats.attack;
        stats.defence          = baseStats.defence;
        stats.creativity       = baseStats.creativity;
        stats.recovery         = baseStats.recovery;
        stats.movementSpeed    = baseStats.movementSpeed;
        stats.cooldownModifier = baseStats.cooldownModifier;

        FullHealHP();
    }

    public override void Die()
    {
        canTakeDamage = false;

        StartCoroutine(DeathRoutine());
    }

    public void DropItem(Item item) 
    {
        //Made this into a seperate function so I can reuse it, for example for a mimic enemy that drops money when it is damaged

        Item newItem = Instantiate(item);

        newItem.transform.position = new Vector3(Random.Range(transform.position.x - lootRadius, transform.position.x + lootRadius),
                                                 Random.Range(transform.position.y - lootRadius, transform.position.y + lootRadius), 0);
    }

    IEnumerator DeathRoutine()
    {
        if (!lootTable) yield break;

        //If quantities or odds list is wrong, drop every item once regardless of rarity
        if (!((lootTable.items.Count == lootTable.quantities.Count) == (lootTable.items.Count == lootTable.odds.Count)))
        {
            Debug.Log("Error with the LootTable, dropping every item once.");
            for (int i = 0; lootTable.items.Count > 0; i++)
            {
                DropItem(lootTable.items[i]);
                yield return new WaitForSeconds(timeBetweenDrops);
            }
        }

        //Else proceed as expected
        else
        {
            for (int i = 0; lootTable.items.Count > i; i++)
            {
                for (int j = 0; lootTable.quantities[i] > j; j++)
                { 
                    //Find a random between 0 and 1, if it is smaller than this item's dropping odds, drop it.
                    if (Random.Range(0.0f, 1.0f) <= lootTable.odds[i])
                    {
                        DropItem(lootTable.items[i]);
                        yield return new WaitForSeconds(timeBetweenDrops);
                    }
                }
            }
        }
        

        base.Die();
    }

    public override void OnHPChanged()
    {
        // UNIMPLEMENTED
        return;
    }

    public override void OnManaChanged()
    {
        // UNIMPLEMENTED
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lootRadius);
    }
}
