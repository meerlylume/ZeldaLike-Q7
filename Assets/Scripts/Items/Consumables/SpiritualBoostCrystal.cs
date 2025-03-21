using UnityEngine;

public class SpiritualBoostCrystal : Consumable
{
    [Header("Creativity/Recovery Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;

    public override void Consume(Stats userStats)
    {
        //Prompt Attack or Defence
        //Prompt which char to put it on

        int amount = Random.Range(minRandomRange, maxRandomRange);
        RaiseCreativity(userStats, amount); //For now, always raise HP
        Debug.Log("Incomplete Implementation: SpiritualBoostCrystal only raises Creativity");
    }

    public void RaiseCreativity(Stats userStats, int amount) { userStats.creativity += amount; }

    public void RaiseRecovery(Stats userStats, int amount)   { userStats.recovery += amount;   }
}
