using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Fight : MonoBehaviour, IFight
{
    [Header("Stats")]
    [SerializeField] protected Stats stats;
    [Space]
    protected Rigidbody2D rb;
    protected float manaCharged;

    [Header("Attacks")]
    [SerializeField] protected List<Attack> attacks;
    protected int  attackIndex   = 0;
    protected bool canTakeDamage = true;
    protected bool isAlive       = true;
    protected bool isInCooldown  = false;
    protected float pushStrength = 2f; //TEMPORARY
    [SerializeField] protected float textLifetime = 1f; //TEMPORARY

    [Header("UI References")]
    [SerializeField] Canvas             worldCanvas;
    [SerializeField] protected GameObject healthBarObject;
                     protected GUIStatBar healthBar;
    [SerializeField] protected GUIStatBar manaBar;
    [SerializeField] GameObject         damageTextPrefab;
    [SerializeField] GameObject         critTextPrefab;
    [Space]

    protected bool canAttack;
    protected Collider2D collider2d;
    protected Anims anims;

    [Space]
    [Header("Sprite")]
    [SerializeField] protected GameObject spriteObject;

    #region Getters
    public virtual bool CanTakeDamage()                { return canTakeDamage;                      }
    public virtual bool IsAlliedWith(Fight fight)      { return fight.stats.isAlly == stats.isAlly; }
    public virtual bool IsAlliedWith(Stats otherStats) { return otherStats.isAlly == stats.isAlly;  }
    public bool GetCanAttack()                         { return canAttack;                          }
    public void SetCanAttack(bool value)               { canAttack = value;                         }
    #endregion

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        healthBar = healthBarObject.GetComponent<GUIStatBar>();
        rb.gravityScale = 0; //in case I forget to put it in the inspector

        FullHealHP();
        ResetStats();
    }

    #region STATS
    public virtual void ResetStats()
    {
        ResetCurrentAttack();
        ResetCurrentDefence();
        ResetCurrentCreativity();
        ResetCurrentRecovery();
    }

    public void ResetCurrentAttack()     { stats.currentATK = stats.attack;     }
    public void ResetCurrentDefence()    { stats.currentDEF = stats.defence;    }
    public void ResetCurrentCreativity() { stats.currentCRE = stats.creativity; }
    public void ResetCurrentRecovery()   { stats.currentRCV = stats.recovery;   }

    public virtual void CopyStatsInto(Stats baseStats, Stats stats)
    {
        stats.name                   = baseStats.name;
        stats.maxHP                  = baseStats.maxHP;
        stats.maxMana                = baseStats.maxMana;
        stats.attack                 = baseStats.attack;
        stats.defence                = baseStats.defence;
        stats.creativity             = baseStats.creativity;
        stats.recovery               = baseStats.recovery;
        stats.movementSpeed          = baseStats.movementSpeed;
        stats.attackCooldownModifier = baseStats.attackCooldownModifier;

        ResetStats();
        FullHealHP();
    }
    #endregion

    public virtual void Attack()
    {
        attacks[attackIndex].PerformAttack(stats, transform.position, manaCharged);
        manaCharged = 0f;
        attackIndex = (attackIndex + 1) % attacks.Count; //increments attackIndex if it's under attacks.Count, else sets it to 0
    }

    public virtual void Die()
    {
        if (!isAlive) return;
        canTakeDamage = false;
        isAlive       = false;

        rb.linearVelocity = Vector2.zero;
        spriteObject.SetActive(false);
        collider2d.enabled = false;
    }

    public virtual void Die(Stats killer) { Die(); }

    protected float CalculateDamage(float atk, bool crit)
    {
        float damage;

        // If attacker crits
        if (crit)
        {
            damage = atk * 2.5f - Random.Range(stats.currentDEF * 0.5f, stats.currentDEF);
            damage = Mathf.Clamp(damage, 0, Mathf.Infinity);
        }

        else
        {
            damage = Random.Range(atk, atk * 1.5f) - Random.Range(stats.currentDEF * 0.5f, stats.currentDEF);
            damage = Mathf.Clamp(damage, 0, Mathf.Infinity);
        }

        if (0.0f < damage && damage < 1.0f) { damage = 1.0f; }
        return damage;
    }

    public virtual void TakeDamage(float atk, bool crit, Vector2 attackPos, Stats attacker)
    {
        if (!isAlive) return;
        if (!canTakeDamage) return;

        float totalDamage = CalculateDamage(atk, crit);

        stats.currentHP = Mathf.Clamp(stats.currentHP - totalDamage, 0, stats.maxHP);

        DamageDisplay(totalDamage, crit, attackPos);
        RefreshHP();

        if (isAlive) anims.SetHurt();

        if (stats.currentHP <= 0) { Die(attacker); }
    }

    protected void DamageDisplay(float totalDamage, bool crit, Vector2 attackPos)
    {
        if (!worldCanvas) return;
        
        GameObject newText = null;

        if (crit) newText = Instantiate(critTextPrefab);   
        else      newText = Instantiate(damageTextPrefab); 

        if (!newText) return;

        // set up the new text
        newText.transform.position = transform.position;
        newText.transform.SetParent(worldCanvas.transform);
        newText.transform.localScale = Vector3.one;

        // call its function
        DamageText damageText = newText.GetComponent<DamageText>();
        Vector2 pushDir       = new Vector2(transform.position.x, transform.position.y) - attackPos;
        int intTotalDamage    = (int)totalDamage;
        damageText.Push(pushDir, textLifetime, pushStrength, intTotalDamage.ToString());
    }

    public virtual void TakeKnockback(float knockback, Vector2 attackPos)
    {
        Vector2 pushDir = new Vector2(transform.position.x, transform.position.y) - attackPos;

        //handle knockback res
        rb.AddForce(pushDir * knockback, ForceMode2D.Impulse);
    }

    public virtual void HealHP(float amount)
    {
        if (!isAlive || amount <= 0) return;
        stats.currentHP = Mathf.Clamp(stats.currentHP + amount * stats.HealingModifier(), stats.currentHP, stats.maxHP);
        RefreshHP();
    }

    public virtual void FullHealHP()
    {
        if (!isAlive) return;
        stats.currentHP = stats.maxHP;
        RefreshHP();
    }

    public virtual void HealMana(float amount)
    {
        if (!isAlive) return;
        stats.currentMana = Mathf.Clamp(stats.currentMana + amount * stats.HealingModifier(), 0, stats.maxMana);
        RefreshMana();
    }

    public virtual void RemoveMana(float amount)
    {
        if (!isAlive) return;
        stats.currentMana = Mathf.Clamp(stats.currentMana - amount, 0, stats.maxMana);
        RefreshMana();
    }

    public virtual void FullHealMana()
    {
        if (!isAlive) return;
        stats.currentMana = stats.maxMana;
        RefreshMana();
    }

    public virtual void FullHeal()
    {
        FullHealMana();
        FullHealHP();
    }

    public virtual IEnumerator AttackRoutine()
    {
        if (isInCooldown) yield break;

        anims.SetAttack();
        isInCooldown = true;
        yield return new WaitForSeconds(attacks[attackIndex].GetCooldown(stats));
        isInCooldown = false;
    }

    #region HealthBar & ManaBar
    public virtual void RefreshHP()   { healthBar.RefreshBar(stats.maxHP, stats.currentHP);   }
    public virtual void RefreshMana() { manaBar.RefreshBar(stats.maxMana, stats.currentMana); }
    #endregion

    #region Stat Raising (item usage)
    public void RaiseMaxHP(float amount)    
    { 
        stats.maxHP     += amount; 
        stats.currentHP += amount;
    }
    public void RaiseMaxMana(float amount)  
    { 
        stats.maxMana     += amount; 
        stats.currentMana += amount;
    }
    public void RaiseAttack(int amount)     
    { 
        stats.attack     += amount; 
        stats.currentATK += amount;
    }
    public void RaiseDefence(int amount)    
    { 
        stats.defence    += amount; 
        stats.currentDEF += amount;
    }
    public void RaiseCreativity(int amount) 
    { 
        stats.creativity += amount; 
        stats.currentCRE += amount;
    }
    public void RaiseRecovery(int amount)   
    { 
        stats.recovery   += amount; 
        stats.currentRCV += amount;
    }
    #endregion
}
