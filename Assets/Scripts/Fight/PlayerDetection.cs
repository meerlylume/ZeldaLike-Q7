using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    private bool isChasing;
    private PlayerFight playerFight;

    #region Get/Set
    public bool GetIsChasing()           { return isChasing;   }
    public void SetIsChasing(bool value) { isChasing = value;  }
    public PlayerFight GetPlayerFight()  { return playerFight; }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerFight)
        {
            collision.TryGetComponent(out PlayerFight enteringCollider);

            if (!enteringCollider) return;

            playerFight = enteringCollider;
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.TryGetComponent(out PlayerFight exitingCollider);

        if (!exitingCollider) return;

        playerFight = null;
        isChasing = false;
    }
}
