using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;
    [Header("Game Over UI")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button restartButton;

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
}
