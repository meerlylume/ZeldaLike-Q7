public interface IFight
{
    bool CanTakeDamage();
    //bool CanDealDamage(); //maybe?

    void TakeDamage(int dmg);

    void Die();

    void HealHP(int amount);

    //To do:
    //Handle drain HP (not necessarily on this interface though)
    //Handle drain Mana (not necessarily on this interface though)
}
