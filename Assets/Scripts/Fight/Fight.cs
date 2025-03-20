using System.Collections.Generic;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Fight : MonoBehaviour, IFight
{
    [Header("Stats")]
    [SerializeField] protected Stats stats;
    [Space]
    protected Rigidbody2D rb;

    [Header("Attacks")]
    [SerializeField] List<Attack> attacks;
    private int attackIndex = 0;

    protected bool canTakeDamage = true;
    protected bool isAlive       = true;
    protected bool isAlly;

    #region Getters
    public virtual bool CanTakeDamage()                { return canTakeDamage;                      }

    public virtual bool IsAlliedWith(Fight fight)      { return fight.stats.isAlly == stats.isAlly; }

    public virtual bool IsAlliedWith(Stats otherStats) { return otherStats.isAlly == stats.isAlly;  }
    #endregion

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; //in case I forget to put it in the inspector

        FullHealHP();
    }
    public void Attack()
    {
        attacks[attackIndex].PerformAttack(stats, transform.position);
        if (attackIndex < attacks.Count - 1) { attackIndex++;   }
        else                                 { attackIndex = 0; }
    }

    public virtual void Die()
    {
        if (!isAlive) return;
        canTakeDamage = false;
        isAlive       = false;

        // /!\ THIS IS ONLY TEMPORARY /!\
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(int dmg)
    {
        //HANDLE DEFENCE AND ARMOR

        if (!isAlive) return;
        if (!canTakeDamage) return;

        Debug.Log("CurrentHP: " + stats.currentHP + " Damage: " + dmg);
        int _totalDamage = Mathf.Clamp(dmg - stats.defence, 0, 999);

        stats.currentHP = Mathf.Clamp(stats.currentHP - _totalDamage, 0, stats.maxHP);
        Debug.Log("CurrentHP: " + stats.currentHP + " TotalDamage: " + _totalDamage);

        OnHPChanged();

        if (stats.currentHP <= 0) { Die(); }
    }

    public virtual void DealDamage(int dmg, IFight target)
    {
        if (!isAlive) return;

        throw new System.NotImplementedException();
    }

    public virtual void HealHP(int amount)
    {
        if (!isAlive) return;
        stats.currentHP = Mathf.Clamp(stats.currentHP + amount, 0, stats.maxHP);
        OnHPChanged();
    }

    public virtual void FullHealHP()
    {
        if (!isAlive) return;
        stats.currentHP = stats.maxHP;
        OnHPChanged();
    }

    public abstract void OnHPChanged();
    public abstract void OnManaChanged();

    public virtual void Attack(Attack attack)
    {
        throw new System.NotImplementedException();
    }
}
