using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanInteract()
    {
        return !isOpen;
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        isOpen = true;
        spriteRenderer.color = Color.red;
    }
}
