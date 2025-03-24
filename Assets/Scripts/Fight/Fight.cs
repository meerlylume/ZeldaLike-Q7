using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Fight : MonoBehaviour, IFight
{
    [Header("Stats")]
    //[SerializeField] protected Stats baseStats;
    [SerializeField] protected Stats stats;
    [Space]
    protected Rigidbody2D rb;

    [Header("Attacks")]
    [SerializeField] protected List<Attack> attacks;
    protected int  attackIndex   = 0;
    protected bool canTakeDamage = true;
    protected bool isAlive       = true;
    protected bool isInCooldown  = false;
    //protected bool isAlly;
    //protected bool isInIFrame   = false; //NOT MVP

    [Header("UI References")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider manaSlider;
    [Space]

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

    public virtual void CopyStatsInto(Stats baseStats, Stats stats)
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

    public virtual void Attack()
    {
        attacks[attackIndex].PerformAttack(stats, transform.position);
        attackIndex = (attackIndex + 1) % attacks.Count; //increments attackIndex if it's under attacks.Count, else sets it to 0
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

        if (stats.name == "Cannelle") return;

        if (spriteRenderer.color == Color.red) spriteRenderer.color = Color.yellow;
        else spriteRenderer.color = Color.red;
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

    public virtual IEnumerator AttackRoutine()
    {
        if (isInCooldown) yield break;

        // Attack
        Attack();
        if (stats.name == "Cannelle") spriteRenderer.color = Color.gray;
        
        //Cooldown
        isInCooldown = true;
        yield return new WaitForSeconds(attacks[attackIndex].GetCooldown(stats));
        if (stats.name == "Cannelle") spriteRenderer.color = Color.white;

        isInCooldown = false;
    }

    #region HealthBar & ManaBar
    public virtual void OnHPChanged()   { RefreshHealthBar(); }

    public virtual void OnManaChanged() { RefreshManaBar();   }

    public virtual void RefreshHealthBar()
    {
        healthSlider.maxValue = stats.maxHP;
        healthSlider.value    = stats.currentHP;
    }

    public virtual void RefreshManaBar()
    {
        manaSlider.maxValue = stats.maxMana;
        manaSlider.value    = stats.currentMana;
    }
    #endregion

}
