using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HTB_Slash : Hitbox
{
    [SerializeField] Vector2 hitboxSize = new Vector2(5, 5);

    public override List<Fight> GetCollidersInHitbox(Vector2 hitboxPos)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(hitboxPos, hitboxSize, 0);
        List<Fight> fighters   = new List<Fight>();

        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].TryGetComponent(out Fight otherFight)) { fighters.Add(otherFight); } }

        return fighters;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, hitboxSize);
    }
}
