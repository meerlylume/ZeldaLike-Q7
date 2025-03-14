using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Header("Health")]
    public int maxHP;
    public int currentHP; 
    [Space]
    public int maxMana;
    public int currentMana; 
    [Space]

    [Header("Character Stats")]
    public int strength;
    public int vitality; 
    //[Space]

    //[Header("Hidden Stats")]
    //public int knockbackResistance;
    //public int knockbackStrength;
}
