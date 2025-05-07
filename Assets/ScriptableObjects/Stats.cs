using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Header("Identity")]
    public new string name;
    public bool isAlly;
    
    [Header("Health")]
    public float maxHP;
    public float currentHP; 
    [Space]
    public float maxMana;
    public float currentMana; 
    [Space]

    [Header("Character Stats")]
    public int attack;
    public int defence; 
    public int creativity; 
    public int recovery;
    [Space]

    [Header("Hidden Stats")] //!\\ WHEN ADDING A STAT, DON'T FORGET TO CHANGE GAMESAVER'S COPY FUNCTION //!\\
    public float movementSpeed;
    public float attackCooldownModifier = 1.0f;

    public bool RollForLuck() { return Random.Range(0, Mathf.Clamp(50 - creativity, 5, Mathf.Infinity)) == 1; }

    public float HealingModifier()
    {
        return Mathf.Clamp(recovery / 10, 1, 2); //TEMPORARY CALCULATION
        //Left a scope because this formula might get more complicated, for example with a status effect that buffs or nerfs healing
    }

    public float KnockbackModifier()
    {
        return attack / 2;
    }

    public float ManaAutoRegenTime()
    {
        return 2 - recovery * 0.1f; //TEMPORARY CALCULATION
    }

    public float LootModifier()
    {
        return Mathf.Clamp(creativity / 50, 0f, 1f);
    }

    public bool IsLootMaxxedOut()
    {
        if (LootModifier() == 1f) return true;

        return false;
    }
}
