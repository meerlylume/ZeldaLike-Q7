using AYellowpaper.SerializedCollections.Editor.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;

    [Header("HealthBar Shake")]
    [SerializeField] private Animator healthBarnimator;
    [SerializeField] private float    healthBarShakeTime;
    [SerializeField] private Animator manAnimator; [Space]

    [Header("Particles")]
    [SerializeField] private ParticleSystem manaParticles; [Space]

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartButton;

    private bool canChargeMana          = true;
    private bool isChargingMana         = false;
    private bool isDroppingMana         = false;
    private bool isAutoRegeneratingMana = false;
    private bool inCombatZone           = false;
    private bool inBreatherZone         = false;

    #region Get/Set
    public Stats GetStats()           { return stats;                                 }
    public void SetPlayerSpeed()      { playerMovement.SetSpeed(stats.movementSpeed); }
    public void SetStats(Stats value) { stats = value;                                }
    #endregion

    public override void Start()
    {
        base.Start();
        playerMovement    = GetComponent<PlayerMovement>();
        playerMovement.SetSpeed(stats.movementSpeed);
        stats.currentMana = stats.maxMana;

        RefreshHP();
        RefreshMana();

        gameOverUI.SetActive(false);
    }

    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (context.started) { StartCoroutine(AttackRoutine()); }
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        playerMovement.FreezePlayerMovement();
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
        GetComponent<SpriteRenderer>().color = Color.red;
        playerMovement.DisablePlayerMovement();
        rb.AddForce(knockbackStrength, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knocktime / 4);
        playerMovement.EnablePlayerMovement();
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ChargeMana(InputAction.CallbackContext context)
    {
        if (!canChargeMana) return;

        if (context.started)
        {
            if (!isChargingMana) StartCoroutine(AutoDropManaRoutine(1f, 1f));
            isChargingMana = true;
            playerMovement.InManaChargingSpeed(true);
            //start mana focusing animation
        }

        if (context.canceled)
        {
            isDroppingMana = false;
            RefreshMana();
            playerMovement.InManaChargingSpeed(false);
            //stop mana focusing animation
        }
    }

    public IEnumerator AutoDropManaRoutine(float time, float amount)
    {
        isDroppingMana = true;
        manAnimator.SetBool("isShaking", true);

        while (stats.currentMana > 0f && isDroppingMana)
        {
            RemoveMana(amount);
            //if (isChargingMana) manaCharged += amount;
            if (!manaParticles.isPlaying) manaParticles.Play();
            yield return new WaitForSeconds(time);
        }

        manaParticles.Stop();
        manAnimator.SetBool("isShaking", false);
        isDroppingMana = false;
        isChargingMana = false;
    }

    public override void RefreshMana()
    {
        base.RefreshMana();
        if ((inBreatherZone || !inCombatZone) && !isChargingMana && !isAutoRegeneratingMana && stats.currentMana != stats.maxMana) 
            StartCoroutine(AutoRegenManaRoutine(stats.ManaAutoRegenTime(), 1f));
    }

    public IEnumerator AutoRegenManaRoutine(float time, float amount)
    {
        if (isDroppingMana) yield break;
        
        isAutoRegeneratingMana = true;

        yield return new WaitForSeconds(time);

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
        yield return new WaitForSeconds(time);
        healthBarnimator.SetBool("isShaking", false);
    }
}