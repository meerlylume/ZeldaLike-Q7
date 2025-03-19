using UnityEngine;

public abstract class Consumable : Item
{
    public abstract void Consume(Stats userStats);
}
