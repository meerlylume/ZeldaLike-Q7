public interface IFight
{
    bool CanTakeDamage();
    //bool CanDealDamage(); //maybe?

    void TakeDamage(float dmg);

    void Die();

    void HealHP(float amount);

    //To do:
    //Handle drain HP (not necessarily on this interface though)
    //Handle drain Mana (not necessarily on this interface though)
}
