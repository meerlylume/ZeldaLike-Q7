using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    [Header("Appearance")]
    public new string name      = "Item";
    public string description   = "Placeholder";
    public Sprite sprite;
    [Space]
    [Header("Properties")]
    public int maxStackQuantity = 64;
    public int basePrice        = 0;
    public bool canBeSold       = true;
    protected bool isPickedUp   = false;
    protected SpriteRenderer spriteRenderer;

    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanInteract() { return !isPickedUp; }

    public void Interact()
    {
        Debug.Log("Interacted with " + name);

        isPickedUp = true;

        spriteRenderer.enabled = false;
    }
}
