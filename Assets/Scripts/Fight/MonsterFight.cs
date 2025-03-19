using System.Collections;
using UnityEngine;

public class MonsterFight : Fight
{
    protected Stats baseStats;
    [Header("Monster Loot")]
    [SerializeField] InventoryData lootTable;
    [SerializeField] float lootRadius = 4f;
    [SerializeField] float timeBetweenDrops = 0.1f;
    private Vector3 deathPos;

    public override void Start()
    {
        baseStats = stats;
        stats = ScriptableObject.CreateInstance<Stats>();
        CopyBaseStats();

        base.Start();
    }
    private void CopyBaseStats()
    {
        stats.name = baseStats.name;
        stats.maxHP = baseStats.maxHP;
        stats.maxMana = baseStats.maxMana;
        stats.attack = baseStats.attack;
        stats.defence = baseStats.defence;
        stats.creativity = baseStats.creativity;
        stats.recovery = baseStats.recovery;

        FullHealHP();
    }

    public override void Die()
    {
        canTakeDamage = false;

        StartCoroutine(DeathRoutine());
    }

    public void DropItem(Item item) 
    {
        //Made this into a seperate function so I can reuse it, for example for a mimic enemy that drops money

        deathPos = transform.position;

        Instantiate(item);
        item.transform.position = new Vector3(Random.Range(deathPos.x - lootRadius, deathPos.x + lootRadius), 
                                              Random.Range(deathPos.y - lootRadius, deathPos.y + lootRadius), 
                                              0);
    }

    IEnumerator DeathRoutine()
    {
        if (!lootTable) yield break;

        for (int i = 0; lootTable.items.Count > i; i++)
        {
            //If quantities list is wrong, drop item once
            if (!(lootTable.items.Count == lootTable.quantities.Count))
            {
                DropItem(lootTable.items[i]);
                yield return new WaitForSeconds(timeBetweenDrops);
            }

            //Else drop everything
            else 
            { 
                for (int j = 0; lootTable.quantities[i] > j; j++)
                {
                    DropItem(lootTable.items[i]);
                    yield return new WaitForSeconds(timeBetweenDrops);
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
}
