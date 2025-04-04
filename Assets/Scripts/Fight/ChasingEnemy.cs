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
        SetIsChasing(false);
    }

    private void FixedUpdate()
    {
        if (!(GetIsChasing() && GetPlayerFight())) return;

        Vector2 direction = GetPlayerFight().transform.position - transform.position;
        rb.AddForce(direction.normalized * speed);

        if ((attacks[attackIndex].CheckIfInRange(stats, transform.position)))
        {
            speed = 0f;
            StartCoroutine(AttackRoutine());
        }
        else
        {
            speed = stats.movementSpeed;
        }
    }
}
