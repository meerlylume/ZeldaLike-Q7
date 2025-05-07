public interface IFight
{
    bool CanTakeDamage();

    void TakeDamage(float atk, bool crit, UnityEngine.Vector2 attackPos, Stats attacker);

    void Die();

    void HealHP(float amount);
}
