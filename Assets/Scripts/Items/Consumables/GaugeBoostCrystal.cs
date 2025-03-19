using UnityEngine;

public class GaugeBoostCrystal : Consumable
{
    [Header("HP/Mana Boost Ranges")]
    public int minRandomRange = 5;
    public int maxRandomRange = 10;

    public override void Consume(Stats userStats)
    {
        //Prompt HP or Mana
        //Prompt which char to put it on

        int amount = Random.Range(minRandomRange, maxRandomRange);
        RaiseHP(userStats, amount); //For now, always raise HP
        Debug.Log("Incomplete Implementation: GaugeBoostCrystal only raises HP");
    }

    public void RaiseHP(Stats userStats, int amount)
    {
        userStats.maxHP       += amount;
        userStats.currentHP   += amount;
    }

    public void RaiseMana(Stats userStats, int amount)
    {
        userStats.maxMana     += amount;
        userStats.currentMana += amount;
    }
}
