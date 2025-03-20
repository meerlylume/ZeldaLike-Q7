using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Header("Identity")]
    public new string name;
    public bool isAlly;
    
    [Header("Health")]
    public int maxHP;
    public int currentHP; 
    [Space]
    public int maxMana;
    public int currentMana; 
    [Space]

    [Header("Character Stats")]
    public int attack;
    public int defence; 
    public int creativity; 
    public int recovery;
    //[Space]

    //hitboxes

    [Header("Hidden Stats")]
    public float movementSpeed;
    //public int knockbackResistance;
    //public int knockbackStrength;
}
