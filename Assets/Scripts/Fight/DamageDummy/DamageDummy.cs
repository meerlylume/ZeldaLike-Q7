using UnityEngine;

public class DamageDummy : EnemyFight
{
    public override void Die() { return; }

    public override void TakeKnockback(float knockback, Vector2 attackPos) { return; }

    public override void RefreshHP() { return; }

    public override void TakeDamage(float atk, bool crit, Vector2 attackPos, Stats attacker)
    {
        if (crit) Debug.Log("CRIT");

        float totalDamage = CalculateDamage(atk, crit);

        DamageDisplay(totalDamage, crit, attackPos);
        RefreshHP();
    }
}
