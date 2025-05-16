using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFight : Fight
{
    //PlayerController playerMovement;

    [Header("HealthBar Shake")]
    [SerializeField] private Animator healthBarnimator;
    [SerializeField] private float    healthBarShakeTime;
    [SerializeField] private Animator manAnimator; [Space]

    [Header("Particles")]
    [SerializeField] private ParticleSystem manaParticles; [Space]

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartButton;

    [HideInInspector] public UnityEvent ManaRanOutEvent;
    [HideInInspector] public UnityEvent<bool> FreezeMovementEvent;
    [HideInInspector] public UnityEvent<bool> DisableMovementEvent;
    [HideInInspector] public UnityEvent<bool> ManaChargingEvent;

    #region Booleans
    private bool canChargeMana          = false;
    private bool isChargingMana         = false;
    private bool isDroppingMana         = false;
    private bool isAutoRegeneratingMana = false;
    private bool inCombatZone           = false;
    private bool inBreatherZone         = false;
    #endregion

    #region Get/Set
    public Stats GetStats()           { return stats;  }
    public void SetStats(Stats value) { stats = value; }

    public void LoadStats(SaveData saveData)
    {
        stats.maxHP       = saveData.maxHP;
        stats.currentHP   = saveData.currentHP;
        stats.maxMana     = saveData.maxMana;
        stats.currentMana = saveData.currentMana;
        stats.attack      = saveData.maxAttack;
        stats.currentATK  = saveData.currentAttack;
        stats.defence     = saveData.maxDefence;
        stats.currentDEF  = saveData.currentDefence;
        stats.creativity  = saveData.maxCreativity;
        stats.currentCRE  = saveData.currentCreativity;
        stats.recovery    = saveData.maxRecovery;
        stats.currentRCV  = saveData.currentRecovery;
    }
    //public void SetPlayerSpeed() { playerMovement.SetSpeed(stats.movementSpeed); }
    public float GetPlayerSpeed() { return stats.movementSpeed; }
    public Anims GetAnims() { return anims;  }
    public bool GetCanChargeMana()           { return canChargeMana;   }
    public void SetCanChargeMana(bool value) {  canChargeMana = value; }
    public bool HasMana() { return stats.currentMana > 0; }
    #endregion

    private void Awake()
    {
        anims = spriteObject.GetComponent<Anims>();
        anims.AttackFrameEvent.AddListener(Attack);
    }

    public override void Start()
    {
        base.Start();
        stats.currentMana = stats.maxMana;

        RefreshHP();
        RefreshMana();

        gameOverUI.SetActive(false);
        canAttack = true;
    }

    public void PlayerAttack()
    {
        if (!canAttack) return;

        StartCoroutine(AttackRoutine());
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        FreezeMovementEvent.Invoke(true);
        GetComponent<SpriteRenderer>().enabled = false;
        gameOverUI.SetActive(true);
        restartButton.Select();
        canTakeDamage = false;
    }

    public override void TakeKnockback(float knockback, Vector2 attackPos)
    {
        Vector2 pushDir = new Vector2(transform.position.x, transform.position.y) - attackPos;

        StartCoroutine(KnockbackRoutine(knockback, pushDir * knockback));
    }

    public IEnumerator KnockbackRoutine(float knocktime, Vector2 knockbackStrength)
    {
        DisableMovementEvent.Invoke(true);

        rb.AddForce(knockbackStrength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knocktime / 4);

        DisableMovementEvent.Invoke(false);
    }

    public void ChargeMana(InputAction.CallbackContext context)
    {
        if (!canChargeMana) return;

        if (context.started)
        {
            if (!isChargingMana) StartCoroutine(AutoDropManaRoutine(1f, 1f, true));
            isChargingMana = true;
            ManaChargingEvent.Invoke(true);
            //start mana focusing animation
        }

        if (context.canceled)
        {
            isDroppingMana = false;
            RefreshMana();
            ManaChargingEvent.Invoke(false);
            //stop mana focusing animation
        }
    }

    public IEnumerator AutoDropManaRoutine(float time, float amount, bool doParticles)
    {
        isDroppingMana = true;
        manAnimator.SetBool("isShaking", true);

        while (stats.currentMana > 0f && isDroppingMana)
        {
            RemoveMana(amount);
            if (isChargingMana) manaCharged += amount * 2;
            if (doParticles && !manaParticles.isPlaying) manaParticles.Play();
            yield return new WaitForSecondsRealtime(time);
        }

        if (stats.currentMana == 0f) ManaRanOutEvent.Invoke();

        manaParticles.Stop();
        manAnimator.SetBool("isShaking", false);
        isDroppingMana = false;
        isChargingMana = false;
    }

    public override void RefreshMana()
    {
        base.RefreshMana();
        if ((inBreatherZone || !inCombatZone) && !isChargingMana && !isAutoRegeneratingMana && stats.currentMana != stats.maxMana) 
            StartCoroutine(AutoRegenManaRoutine(stats.ManaAutoRegenTime(), stats.ManaAutoRegenAmount()));
    }

    public IEnumerator AutoRegenManaRoutine(float time, float amount)
    {
        if (isDroppingMana) yield break;
        
        isAutoRegeneratingMana = true;

        yield return new WaitForSecondsRealtime(time);

        isAutoRegeneratingMana = false;
        HealMana(amount);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        if (collision.tag == "NoManaRegenZone") inCombatZone   = true;

        if (collision.tag == "BreatherZone")    inBreatherZone = true;

        RefreshMana(); //in case the player enters a zone that lets them regen
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (collision.tag == "NoManaRegenZone") inCombatZone   = false;

        if (collision.tag == "BreatherZone")    inBreatherZone = false;

        RefreshMana(); //in case the player leaves a no regen zone
    }

    public override void HealHP(float amount)
    {
        if (!isAlive || amount <= 0) return;

        float breatherModifier = 1f;
        if (inBreatherZone) breatherModifier = 2f;

        stats.currentHP = Mathf.Clamp(stats.currentHP + amount * stats.HealingModifier() * breatherModifier, stats.currentHP, stats.maxHP);
        RefreshHP();
    }

    public override void TakeDamage(float atk, bool crit, Vector2 attackPos, Stats attacker)
    {
        base.TakeDamage(atk, crit, attackPos, attacker);

        StartCoroutine(HealthBarShakeRoutine(healthBarShakeTime));
    }

    public IEnumerator HealthBarShakeRoutine(float time)
    {
        healthBarnimator.SetBool("isShaking", true);
        yield return new WaitForSecondsRealtime(time);
        healthBarnimator.SetBool("isShaking", false);
    }

    public bool DoDefaultMenu() { return (!inCombatZone || inBreatherZone); }

    public void OnMenuOpen()
    {
        StartCoroutine(AutoDropManaRoutine(1f, 1f, false));
        isChargingMana = false;
    }

    public void OnMenuClose()
    {
        isDroppingMana = false;
        isChargingMana = false;
    }

    public void GODMODE()
    {
        stats.maxHP = 9999999f;
        stats.currentHP = 9999999f;
        stats.maxMana = 9999999f;
        stats.currentMana = 9999999f;
        stats.attack = 9999999;
        stats.currentATK = 9999999;
        stats.defence = 9999999;
        stats.currentDEF = 9999999;
        stats.creativity = 9999999;
        stats.currentCRE = 9999999;
        stats.recovery = 9999999;
        stats.currentRCV = 9999999;
    }
}