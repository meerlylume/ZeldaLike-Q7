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

    private void Start()
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
        if (!CanInteract()) return;
        isOpen = true;
        CheckIfOpen();
    }

    public InventoryData GetInventory() { return inventory; }
}

[System.Serializable]
public class ChestData
{
    public int ID;
    public bool IsOpen;
}