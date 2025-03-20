using UnityEngine;

public class PhysicalBoostCrystal : Consumable
{
    [Header("Strength/Defence Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;

    public override void Consume(Stats userStats)
    {
        //Prompt Attack or Defence
        //Prompt which char to put it on

        int amount = Random.Range(minRandomRange, maxRandomRange);
        RaiseAttack(userStats, amount); //For now, always raise HP
        Debug.Log("Incomplete Implementation: PhysicalBoostCrystal only raises Attack");
    }

    public void RaiseAttack(Stats userStats, int amount)  { userStats.attack += amount; }

    public void RaiseDefence(Stats userStats, int amount) { userStats.defence += amount; }
}
