using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ATK_Slash : Attack
{
    public override void PerformAttack(Stats stats, Vector2 attackPos)
    {
        foreach (Fight fighter in hitbox.GetCollidersInHitbox(attackPos))
        {
            if (fighter.IsAlliedWith(stats)) return;

            Debug.Log("Attacked with " + stats.attack + " attack");
            fighter.TakeDamage(stats.attack);
        }
    }
}
