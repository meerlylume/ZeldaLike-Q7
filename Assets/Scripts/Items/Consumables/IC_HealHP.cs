using UnityEngine;

public class IC_HealHP : Consumable
{
    [Header("Effects")]
    [SerializeField] protected int hpHealed;
    public override void Consume(Fight user) { user.HealHP(hpHealed); }
}
