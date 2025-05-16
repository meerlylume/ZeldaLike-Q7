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
    public int currentATK;
    [Space]
    public int defence; 
    public int currentDEF;
    [Space]
    public int creativity;
    public int currentCRE;
    [Space]
    public int recovery;
    public int currentRCV;
    [Space]

    [Header("Hidden Stats")] //!\\ WHEN ADDING A STAT, DON'T FORGET TO CHANGE GAMESAVER'S COPY FUNCTION //!\\
    public float movementSpeed;
    public float attackCooldownModifier = 1.0f;

    public bool RollForCrit() 
    { 
        return Random.Range(0, Mathf.Clamp(50 - currentCRE, 0, Mathf.Infinity)) <= 1;
    }

    public float HealingModifier()
    {
        return Mathf.Clamp(currentRCV / 10, 1, 2);
    }

    public float KnockbackModifier()
    {
        return currentATK / 2;
    }

    public float ManaAutoRegenTime()
    {
        return 2 - currentRCV * 0.1f; //TEMPORARY CALCULATION
    }

    public float LootModifier()
    {
        return Mathf.Clamp(currentCRE / 50, 0f, 1f);
    }

    public bool IsLootMaxxedOut()
    {
        if (LootModifier() == 1f) return true;

        return false;
    }

    public float ManaAutoRegenAmount()
    {
        return 1f + (currentCRE * 0.1f);
    }
}
