using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFight : MonoBehaviour, IFight
{
    [SerializeField] Stats stats;
    PlayerMovement playerMovement;
    private bool canTakeDamage = true;

    //List<GameObject> defendersInRange;
    HashSet<GameObject> defendersInRange;

    public bool CanTakeDamage() { return canTakeDamage; }

    private void Start()
    {
        stats.currentHP   = stats.maxHP;
        stats.currentMana = stats.maxMana;

        playerMovement    = transform.parent.GetComponent<PlayerMovement>();
    }

    public void TryAttack(InputAction.CallbackContext context)
    {
        //if (Physics2D.OverlapBox(_groundCheckPos.position, _groundCheckSize, 0, _groundLayer)) return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IFight defender) && defender.CanTakeDamage())
        {
            //defendersInRange.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IFight defender) && defender.CanTakeDamage())
        {
            //if ()
        }
    }

    public void TakeDamage(int dmg)
    {
        stats.currentHP -= dmg;
        if (stats.currentHP < 0) { Die(); }
    }

    private void Die()
    {
        if (playerMovement) playerMovement.DisablePlayerMovement();
    }

}
