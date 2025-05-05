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

    [Header("Attacks")]
    [SerializeField] protected List<Attack> attacks;
    protected int  attackIndex   = 0;
    protected bool canTakeDamage = true;
    protected bool isAlive       = true;
    protected bool isInCooldown  = false;
    protected float pushStrength = 2f; //TEMPORARY
    [SerializeField] protected float textLifetime = 1f; //TEMPORARY

    [Header("UI References")]
    [SerializeField] Canvas     worldCanvas;
    [SerializeField] Slider     healthSlider;
    [SerializeField] Slider     manaSlider;
    [SerializeField] GameObject damageTextPrefab;
    [SerializeField] GameObject critTextPrefab;
    [SerializeField] GameObject parryTextPrefab;
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

    private float CalculateDamage(float atk, bool crit, bool parry)
    {
        float damage;

        // If defender parries
        if (!crit && parry) { return 0.0f; }

        // If attacker crits
        else if (crit && !parry)
        {
            damage = atk * 2.5f - Random.Range(stats.defence * 0.5f, stats.defence);
            damage = Mathf.Clamp(damage, 0, Mathf.Infinity);
        }

        // If neither crit nor parry, or both do
        else
        {
            damage = Random.Range(atk, atk * 1.5f) - Random.Range(stats.defence * 0.5f, stats.defence);
            damage = Mathf.Clamp(damage, 0, Mathf.Infinity);
        }

        if (0.0f < damage && damage < 1.0f) { damage = 1.0f; }
        return damage;
    }

    public virtual void TakeDamage(float atk, bool crit, Vector2 attackPos)
    {
        if (!isAlive) return;
        if (!canTakeDamage) return;


        bool parry = stats.RollForLuck();

        float totalDamage = CalculateDamage(atk, crit, parry);

        stats.currentHP = Mathf.Clamp(stats.currentHP - totalDamage, 0, stats.maxHP);

        DamageDisplay(totalDamage, crit, parry, attackPos);
        OnHPChanged();

        if (stats.currentHP <= 0) { Die(); }
    }

    private void DamageDisplay(float totalDamage, bool crit, bool parry, Vector2 attackPos)
    {
        if (!worldCanvas) return;
        
        GameObject newText = null;

        // if neither crit nor parry or both crit and parry
        if (!(crit ^ parry)) { newText = Instantiate(damageTextPrefab); }
        // if crit only
        else if (crit)       { newText = Instantiate(critTextPrefab);   }
        // if parry only
        else if (parry)      { newText = Instantiate(parryTextPrefab);  }

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
        OnHPChanged();
    }

    public virtual void FullHealHP()
    {
        if (!isAlive) return;
        stats.currentHP = stats.maxHP;
        OnHPChanged();
    }

    public virtual void HealMana(float amount)
    {
        if (!isAlive) return;
        stats.currentMana = Mathf.Clamp(stats.currentMana + amount * stats.HealingModifier(), 0, stats.maxMana);
        OnManaChanged();
    }

    public virtual void RemoveMana(float amount)
    {
        if (!isAlive) return;
        stats.currentMana = Mathf.Clamp(stats.currentMana - amount, 0, stats.maxMana);
        OnManaChanged();
    }

    public virtual void FullHealMana()
    {
        if (!isAlive) return;
        stats.currentMana = stats.maxMana;
        OnManaChanged();
    }

    public virtual void FullHeal()
    {
        FullHealMana();
        FullHealHP();
    }

    public virtual IEnumerator AttackRoutine()
    {
        if (isInCooldown) yield break;

        // Attack
        Attack();
        if (stats.name == "Cannelle") spriteRenderer.color = Color.gray; //temporary
        
        //Cooldown
        isInCooldown = true;
        yield return new WaitForSeconds(attacks[attackIndex].GetCooldown(stats));
        if (stats.name == "Cannelle") spriteRenderer.color = Color.white; //temporary

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
    public void RaiseAttack(int amount)     { stats.attack     += amount; }
    public void RaiseDefence(int amount)    { stats.defence    += amount; }
    public void RaiseCreativity(int amount) { stats.creativity += amount; }
    public void RaiseRecovery(int amount)   { stats.recovery   += amount; }
    #endregion
}
