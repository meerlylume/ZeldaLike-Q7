using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] protected Hitbox hitbox;
    //[SerializeField] float attackModifier;
    public abstract void PerformAttack(Stats stats, Vector2 attackPos);
}
