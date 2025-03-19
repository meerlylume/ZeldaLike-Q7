using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;

    [SerializeField] Transform testHitboxPos;
    [SerializeField] Vector3 testHitboxSize;

    HashSet<Collider2D> defendersInRange;

    public Stats GetStats() { return stats; }

    public override void Start()
    {
        base.Start();
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(testHitboxPos.position, testHitboxSize, 0);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out MonsterFight monsterFight))
                {
                    Debug.Log("Attacked with " + stats.attack + " attack");
                    monsterFight.TakeDamage(stats.attack);
                }
            }
        }
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        playerMovement.DisablePlayerMovement();
        base.Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(testHitboxPos.position, testHitboxSize);
    }
}
