using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Fight : MonoBehaviour, IFight
{
    [Header("Stats")]
    [SerializeField] protected Stats stats;
    [Space]
    protected Rigidbody2D rb;

    protected bool canTakeDamage = true;
    protected bool isAlive       = true;

    public virtual bool CanTakeDamage() { return canTakeDamage; } //HANDLE ALLIES

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; //in case I forget to put it in the inspector

        FullHealHP();
    }

    public virtual void Die()
    {
        if (!isAlive) return;
        canTakeDamage = false;
        isAlive       = false;

        // /!\ THIS IS ONLY TEMPORARY /!\
        gameObject.SetActive(false);
    }

    public virtual void TakeDamage(int dmg)
    {
        //HANDLE DEFENCE AND ARMOR

        if (!isAlive) return;
        if (!canTakeDamage) return;

        Debug.Log("CurrentHP: " + stats.currentHP + " Damage: " + dmg);
        int _totalDamage = Mathf.Clamp(dmg - stats.defence, 0, 999);

        stats.currentHP = Mathf.Clamp(stats.currentHP - _totalDamage, 0, stats.maxHP);
        Debug.Log("CurrentHP: " + stats.currentHP + " TotalDamage: " + _totalDamage);

        if (stats.currentHP <= 0) { Die(); }
    }

    public virtual void DealDamage(int dmg, IFight target)
    {
        if (!isAlive) return;

        throw new System.NotImplementedException();
    }

    public virtual void HealHP(int amount)
    {
        if (!isAlive) return;
        stats.currentHP = Mathf.Clamp(stats.currentHP + amount, 0, stats.maxHP);
    }

    public virtual void FullHealHP()
    {
        if (!isAlive) return;
        stats.currentHP = stats.maxHP;
    }
}
