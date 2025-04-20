using System.Collections.Generic;
using UnityEngine;

public abstract class Hitbox : MonoBehaviour
{
    protected Vector2 hitboxPos;
    public abstract List<Fight> GetCollidersInHitbox(Vector2 hitboxPos);
}
