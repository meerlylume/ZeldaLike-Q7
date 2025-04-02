using UnityEngine;

public class IC_SpiritualBoostCrystal : Consumable
{
    [Header("Creativity/Recovery Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;

    public override void Consume(Fight user)
    {
        //Prompt Attack or Defence

        int amount = Random.Range(minRandomRange, maxRandomRange);
        user.RaiseCreativity(amount);
        //user.RaiseRecovery(amount);
        Debug.Log("Incomplete Implementation: SpiritualBoostCrystal only raises Creativity");
    }
}
