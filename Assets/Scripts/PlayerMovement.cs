using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed = 5f; //Only if PlayerFight failed to set this value
    private Vector2 moveInput;
    private bool canMove;
    private Rigidbody2D rb;
    private Vector2 knockbackForce;

    #region Get/Set
    public void SetSpeed(float value) { moveSpeed = value; }
    public void    SetKnockbackForce(Vector2 value) { knockbackForce = value; }
    public Vector2 GetKnockbackForce()              { return knockbackForce;  }
    #endregion

    #region Freeze/Unfreeze Player Movement (meant for dialogue & UI)
    public void FreezePlayerMovement()
    {
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        moveInput = Vector2.zero;
        rb.linearVelocity = moveInput;
    }

    public void UnfreezePlayerMovement() 
    {
        canMove = true; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    #endregion

    #region Enable/Disable Player Movement (meant for combat, knockback, and stun)
    public void DisablePlayerMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
    }

    public void EnablePlayerMovement()
    {
        canMove = true;
        rb.linearVelocity = Vector2.zero;
    }

    #endregion

    private void Start()
    {
        rb      = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    private void FixedUpdate() 
    { 
        if (canMove) rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }
}
