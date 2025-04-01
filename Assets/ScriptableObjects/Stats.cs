using UnityEditor.Experimental.GraphView;
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
}
