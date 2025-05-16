using UnityEngine;

public class Chest : Inventory, IInteractable
{
    [Header("Sprites")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private int ID;
    public static int lastID;

    #region Get/Set
    public int  GetID()     { return ID;     }
    public bool GetIsOpen() { return isOpen; }
    #endregion

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastID++;
        ID = lastID;

        CheckIfOpen();
    }

    public void CheckIfOpen()
    {
        if (isOpen) spriteRenderer.sprite = openSprite;
        else        spriteRenderer.sprite = closedSprite;
    }

    public bool CanInteract() { return !isOpen; }

    public void Interact()
    {
        
    }

    public InventoryData GetInventory() { return inventory; }

    public void Interact(InteractionDetector interactor)
    {
        if (!CanInteract()) return;
        interactor.ChestInteract(this);
        isOpen = true;
        CheckIfOpen();
    }
}

[System.Serializable]
public class ChestData
{
    public int ID;
    public bool IsOpen;
}