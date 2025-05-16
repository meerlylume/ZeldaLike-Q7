using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float moveSpeed;
    private float speedModifier;
    private Vector2 moveInput;
    private bool canMove;
    private Rigidbody2D rb;
    private Vector2 knockbackForce;
    private Anims anims;

    #region Get/Set
    public void    SetSpeed(float value)            { moveSpeed      = value; }
    public void    SetAnims(Anims value)            { anims          = value; }
    public void    InManaChargingSpeed(bool value)    { 
        if (value) speedModifier = 0.5f;
        else       speedModifier = 1f;
    }
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
        speedModifier = 1f;
        canMove = true;
    }

    private void FixedUpdate() 
    { 
        if (canMove) rb.linearVelocity = moveInput.normalized * moveSpeed * speedModifier;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x != 0) anims.SetFlipSprite(moveInput.x < 0);

        rb.linearVelocity = moveInput.normalized * moveSpeed * speedModifier;
    }
}