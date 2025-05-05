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
    private bool canAutoRegenMana       = true;

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

        Debug.Log("Player took knockback. A feedback is necessary because oh my god.");
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
            OnManaChanged();
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
        Debug.Log("End of Routine");
    }

    public override void OnManaChanged()
    {
        base.OnManaChanged();
        if (stats.currentMana != stats.maxMana && !isFocusingMana && !isAutoRegeneratingMana) 
            StartCoroutine(AutoRegenManaRoutine(stats.ManaAutoRegenTime(), 1f));
    }

    public IEnumerator AutoRegenManaRoutine(float time, float amount)
    {
        if (!canAutoRegenMana) yield break;
        isAutoRegeneratingMana = true;

        yield return new WaitForSeconds(time);

        isAutoRegeneratingMana = false;
        HealMana(amount);
    }
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.tag == "NoManaRegen") canAutoRegenMana = false; }
    private void OnTriggerExit2D(Collider2D collision) { if (collision.tag == "NoManaRegen") canAutoRegenMana = true; }
}