using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerFight : Fight
{
    PlayerMovement playerMovement;

    [Header("UI References")]
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider manaSlider;

    public Stats GetStats() { return stats; }
    public void SetPlayerSpeed() { playerMovement.SetSpeed(stats.movementSpeed); }

    public override void Start()
    {
        base.Start();
        playerMovement    = GetComponent<PlayerMovement>();
        stats.currentMana = stats.maxMana;

        RefreshHealthBar();
        RefreshManaBar();
    }

    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (context.started) { StartCoroutine(AttackRoutine()); }
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        playerMovement.DisablePlayerMovement();
        base.Die();
    }

    #region HealthBar & ManaBar
    public void RefreshHealthBar()
    {
        if (stats.maxHP == 0) return;
        healthSlider.value = stats.currentHP / stats.maxHP;

    }

    public void RefreshManaBar()
    {
        if (stats.maxMana == 0) return;
        manaSlider.value = stats.currentMana / stats.maxMana;
    }

    public override void OnHPChanged()   { RefreshHealthBar(); }

    public override void OnManaChanged() { RefreshManaBar();   }
    #endregion
}
