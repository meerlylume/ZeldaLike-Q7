using System.Collections;
using UnityEngine;

public class ChasingEnemy : EnemyFight
{
    [SerializeField] PlayerDetection playerDetection;
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

        anims = spriteObject.GetComponent<Anims>();

        SetIsChasing(false);
        anims.AttackFrameEvent.AddListener(Attack);
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        if (!(GetIsChasing() && GetPlayerFight())) return;

        Vector2 direction = GetPlayerFight().transform.position - transform.position;
        rb.AddForce(direction.normalized * speed);
        anims.SetFlipSprite(direction.x <= 0f);

        if ((attacks[attackIndex].CheckIfInRange(stats, transform.position)))
        {
            speed = 0f;
            StartCoroutine(AttackRoutine());
        }
        else { speed = stats.movementSpeed; }
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
