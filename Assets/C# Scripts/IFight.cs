public interface IFight
{
    bool CanTakeDamage();
    //bool CanDealDamage();

    void TakeDamage(int dmg);

    void DealDamage(int dmg, IFight target);

    void Die();

    void HealHP(int amount);

    //Handle drain HP (not necessarily on this interface though)
    //Handle drain Mana (not necessarily on this interface though)
}
