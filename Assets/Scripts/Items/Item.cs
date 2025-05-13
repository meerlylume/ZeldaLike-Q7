using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    [Header("Appearance")]
    public new string name      = "Item";
    public string description   = "Placeholder Description";
    public string information   = "Placeholder Information";
    public Sprite sprite;
    [SerializeField] protected Sprite highlightedSprite;
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

    public void Interact(InteractionDetector interactor)
    {
        if (!CanInteract()) return;

        isPickedUp = true;

        spriteRenderer.enabled = false;

        interactor.ItemInteract(this);
    }

    public void HighlightSprite(bool highlight)
    {
        if (!highlightedSprite) return;

        if (highlight) spriteRenderer.sprite = highlightedSprite;
        else           spriteRenderer.sprite = sprite;
    }

    public void Interact()
    {
        //throw new System.NotImplementedException();
    }
}
