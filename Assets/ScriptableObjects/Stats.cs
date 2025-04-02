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

    [Header("Hidden Stats")]
    public float movementSpeed;
    public float cooldownModifier = 1.0f;
    //NOT MVP:
    //public float knockbackResistance;
    //public float knockbackStrength;

    public bool RollForLuck() { return Random.Range(0, Mathf.Clamp(50 - creativity, 5, Mathf.Infinity)) == 1; }

    public float HealingModifier()
    {
        return Mathf.Clamp(recovery / 10, 1, 2); //TEMPORARY CALCULATION
        //Left a scope because this formula might get more complicated, for example with a status effect that buffs or nerfs healing
    }
}
