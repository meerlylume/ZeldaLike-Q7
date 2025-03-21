using System.Collections;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [Header("Hitbox Reference")]
    [SerializeField] protected Hitbox hitbox;
    [Space]

    [Header("Attack Properties")]
    [SerializeField] protected float cooldown;
    [SerializeField] protected float knockback;
    protected bool isInCooldown;

    public abstract void PerformAttack(Stats stats, Vector2 attackPos);

    public abstract bool CheckIfInRange(Stats stats, Vector2 attackPos);

    public virtual float GetCooldown(Stats stats) { return cooldown * stats.cooldownModifier; }
}
