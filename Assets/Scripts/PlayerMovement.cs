using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private bool canMove;
    private Rigidbody2D rb;

    public void SetSpeed(float value) { moveSpeed = value; }

    public void DisablePlayerMovement()
    {
        canMove = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EnablePlayerMovement() 
    {
        canMove = true; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMove = true;
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove) 
        { 
            moveInput = Vector2.zero; 
            rb.linearVelocity = moveInput;
            return;
        }

        moveInput = context.ReadValue<Vector2>();
        
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private void TakeKnockback()
    {
        throw new NotImplementedException();
    }
}
