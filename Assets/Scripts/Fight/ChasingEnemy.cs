using System.Collections;
using UnityEngine;

public class ChasingEnemy : EnemyFight
{
    [SerializeField] PlayerDetection playerDetection;
    [SerializeField] GameObject spriteObject;
    ChasingAnims chasingAnims;
    private float speed;

    #region Because I was tired of writing "playerDetection." everytime
    private PlayerFight GetPlayerFight()  { return playerDetection.GetPlayerFight(); }
    private bool GetIsChasing()           { return playerDetection.GetIsChasing();   }
    private void SetIsChasing(bool value) { playerDetection.SetIsChasing(value);     }
    #endregion

    public override void Start()
    {
        base.Start();

        speed = stats.movementSpeed;

        chasingAnims = spriteObject.GetComponent<ChasingAnims>();

        SetIsChasing(false);
        chasingAnims.AttackFrameEvent.AddListener(Attack);
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        if (!(GetIsChasing() && GetPlayerFight())) return;

        Vector2 direction = GetPlayerFight().transform.position - transform.position;
        rb.AddForce(direction.normalized * speed);
        chasingAnims.SetFlipSprite(direction.x <= 0f);

        if ((attacks[attackIndex].CheckIfInRange(stats, transform.position)))
        {
            speed = 0f;
            StartCoroutine(AttackRoutine());
        }
        else { speed = stats.movementSpeed; }
    }

    public override IEnumerator AttackRoutine()
    {
        if (isInCooldown) yield break;

        chasingAnims.SetAttack();
        isInCooldown = true;
        yield return new WaitForSeconds(attacks[attackIndex].GetCooldown(stats));
        isInCooldown = false;
    }

    public override void TakeDamage(float atk, bool crit, Vector2 attackPos, Stats attacker)
    {
        base.TakeDamage(atk, crit, attackPos, attacker);
        chasingAnims.SetFlipSprite(!chasingAnims.GetFlipSprite());
        if (isAlive) chasingAnims.SetHurt();
    }

    public override void Die()
    {
        if (!isAlive) return;
        canTakeDamage = false;
        isAlive = false;

        spriteObject.SetActive(false);
        rb.linearVelocity = Vector2.zero;
        collider2d.enabled = false;
    }

    public override void Respawn()
    {
        isAlive = true;

        healthBarObject.SetActive(true);
        canTakeDamage      = true;
        collider2d.enabled = true;
        spriteObject.SetActive(true);
        rb.linearVelocity  = Vector2.zero;
        transform.position = spawnPos;

        FullHeal();
    }
}
