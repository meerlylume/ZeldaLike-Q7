using UnityEngine.InputSystem;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;

    public Stats GetStats()      { return stats; }
    public void SetPlayerSpeed() { playerMovement.SetSpeed(stats.movementSpeed); }

    public override void Start()
    {
        base.Start();
        playerMovement    = GetComponent<PlayerMovement>();
        stats.currentMana = stats.maxMana;

        RefreshHealthBar();
        RefreshManaBar();
    }

    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (context.started) { StartCoroutine(AttackRoutine()); }
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        playerMovement.DisablePlayerMovement();
        base.Die();
    }
}
