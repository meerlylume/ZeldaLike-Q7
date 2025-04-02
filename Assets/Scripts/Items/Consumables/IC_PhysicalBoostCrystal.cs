using UnityEngine;

public class IC_PhysicalBoostCrystal : Consumable
{
    [Header("Strength/Defence Boost Ranges")]
    public int minRandomRange = 1;
    public int maxRandomRange = 3;

    public override void Consume(Fight user)
    {
        //Prompt Attack or Defence
        //Prompt which char to put it on

        int amount = Random.Range(minRandomRange, maxRandomRange);
        user.RaiseAttack(amount);
        //user.RaiseDefence(amount);
        Debug.Log("Incomplete Implementation: PhysicalBoostCrystal only raises Attack");
    }
}
