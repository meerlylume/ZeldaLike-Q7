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
    //[Space]

    //hitboxes

    [Header("Hidden Stats")]
    public float movementSpeed;
    public float cooldownModifier = 1.0f;
    //public int knockbackResistance;
    //public int knockbackStrength;

    public bool RollForLuck()
    {
        //GDD: 50 - attacker.creativity;  min 5, no max

        int odds = Mathf.Clamp(50 - creativity, 5, 999999);

        return Random.Range(0, odds) == 1;
    }
}
