using UnityEngine;

public class IC_GaugeBoostCrystal : Consumable
{
    [Header("HP/Mana Boost Ranges")]
    public int minRandomRange = 5;
    public int maxRandomRange = 10;

    public override void Consume(Fight user)
    {
        //Prompt HP or Mana
        //Prompt which char to put it on

        int amount = Random.Range(minRandomRange, maxRandomRange);
        user.RaiseMaxHP(amount);
        //user.RaiseMaxMana(amount);
        Debug.Log("Incomplete Implementation: GaugeBoostCrystal only raises HP");
    }
}

