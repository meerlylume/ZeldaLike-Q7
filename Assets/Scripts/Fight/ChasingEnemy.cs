using System.Collections;
using UnityEngine;

public class ChasingEnemy : EnemyFight
{
    [SerializeField] PlayerDetection playerDetection;

    #region Because I was tired of writing "playerDetection." everytime
    private PlayerFight GetPlayerFight()  { return playerDetection.GetPlayerFight(); }
    private bool GetIsChasing()           { return playerDetection.GetIsChasing();   }
    private void SetIsChasing(bool value) { playerDetection.SetIsChasing(value);     }
    #endregion

    public override void Start()
    {
        base.Start();

        SetIsChasing(false);
    }

    private void FixedUpdate()
    {
        if (!(GetIsChasing() && GetPlayerFight())) return;

        Vector2 direction = GetPlayerFight().transform.position - transform.position;
        rb.linearVelocity = direction.normalized * stats.movementSpeed;

        if (!(attacks[attackIndex].CheckIfInRange(stats, transform.position))) return;

        StartCoroutine(AttackRoutine());
    }
}
