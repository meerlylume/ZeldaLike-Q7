using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ATK_Slash : Attack
{

    public override void PerformAttack(Stats stats, Vector2 attackPos, float manaDamage)
    {
        foreach (Fight fighter in hitbox.GetCollidersInHitbox(attackPos))
        {
            if (fighter.IsAlliedWith(stats)) continue;

            fighter.TakeDamage(stats.currentATK + manaDamage, stats.RollForCrit(), attackPos, stats);
            fighter.TakeKnockback(knockback /** stats.KnockbackModifier()*/, attackPos);
        }
    }

    public override bool CheckIfInRange(Stats stats, Vector2 attackPos)
    {
        foreach (Fight fighter in hitbox.GetCollidersInHitbox(attackPos))
        { if (!fighter.IsAlliedWith(stats)) return true; }

        return false;
    }
}
