using AYellowpaper.SerializedCollections.Editor.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;

    [Header("Particles")]
    [SerializeField] private ParticleSystem manaParticles; [Space]

    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartButton;

    private bool isFocusingMana         = false;
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
        stats.currentMana = stats.maxMana;

        RefreshHealthBar();
        RefreshManaBar();

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

    public void FocusMana(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isFocusingMana) StartCoroutine(AutoDropManaRoutine(1f, 1f));
            //start mana focusing animation
        }

        if (context.canceled)
        {
            isFocusingMana = false;
            RefreshMana();
            //stop mana focusing animation
        }
    }

    public IEnumerator AutoDropManaRoutine(float time, float amount)
    {
        isFocusingMana = true;

        while (stats.currentMana > 0f && isFocusingMana)
        {
            RemoveMana(amount);
            if (!manaParticles.isPlaying) manaParticles.Play();
            yield return new WaitForSeconds(time);
        }

        manaParticles.Stop();
        isFocusingMana = false;
    }

    public override void RefreshMana()
    {
        base.RefreshMana();
        if ((inBreatherZone || !inCombatZone) && !isFocusingMana && !isAutoRegeneratingMana && stats.currentMana != stats.maxMana) 
            StartCoroutine(AutoRegenManaRoutine(stats.ManaAutoRegenTime(), 1f));
    }

    public IEnumerator AutoRegenManaRoutine(float time, float amount)
    {
        isAutoRegeneratingMana = true;

        yield return new WaitForSeconds(time);

        isAutoRegeneratingMana = false;
        HealMana(amount);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    { 
        if      (collision.tag == "NoManaRegenZone") inCombatZone   = true;

        else if (collision.tag == "BreatherZone")    inBreatherZone = true;

        RefreshMana(); //in case the player enters a zone that lets them regen
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if      (collision.tag == "NoManaRegenZone") inCombatZone   = false;

        else if (collision.tag == "BreatherZone")    inBreatherZone = false;
    }

    public override void HealHP(float amount)
    {
        if (!isAlive || amount <= 0) return;

        float breatherModifier = 1f;
        if (inBreatherZone) breatherModifier = 2f;

        stats.currentHP = Mathf.Clamp(stats.currentHP + amount * stats.HealingModifier() * breatherModifier, stats.currentHP, stats.maxHP);
        RefreshHP();
    }
}