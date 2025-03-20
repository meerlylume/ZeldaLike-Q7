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
        if (context.started) { Attack(); }
    }

    public override void Die()
    {
        //Stop Movement
        //Gameover Coroutine

        playerMovement.DisablePlayerMovement();
        base.Die();
    }

    public void RefreshHealthBar()
    {
        if (stats.currentHP == 0) return;
        healthSlider.value = stats.maxHP / stats.currentHP;
    }

    public void RefreshManaBar()
    {
        if (stats.currentMana == 0) return;
        manaSlider.value   = stats.maxMana / stats.currentMana;
    }

    public override void OnHPChanged()   { RefreshHealthBar(); }

    public override void OnManaChanged() { RefreshManaBar();   }
}
