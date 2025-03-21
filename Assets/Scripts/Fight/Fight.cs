using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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
    [SerializeField] protected List<Attack> attacks;
    protected int attackIndex = 0;

    protected bool canTakeDamage = true;
    protected bool isAlive       = true;
    protected bool isAlly;
    protected bool isInCooldown  = false;
    //protected bool isInIFrame   = false; //NOT MVP

    [Space]
    [Header("TEMPORARY")]
    [SerializeField] SpriteRenderer spriteRenderer;

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
    public virtual void Attack()
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

    public virtual void TakeDamage(float dmg)
    {
        //DAMAGE FORKS

        if (!isAlive) return;
        if (!canTakeDamage) return;

        float _totalDamage = Mathf.Clamp(dmg - stats.defence, 0, 999);

        stats.currentHP = Mathf.Clamp(stats.currentHP - _totalDamage, 0, stats.maxHP);

        OnHPChanged();

        if (stats.currentHP <= 0) { Die(); }
    }

    public virtual void HealHP(float amount)
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

    public virtual IEnumerator AttackRoutine()
    {
        if (isInCooldown) yield break;

        Attack();
        spriteRenderer.color = Color.gray;
        isInCooldown = true;

        yield return new WaitForSeconds(attacks[attackIndex].GetCooldown(stats));
        spriteRenderer.color = Color.white;

        isInCooldown = false;
}
}
