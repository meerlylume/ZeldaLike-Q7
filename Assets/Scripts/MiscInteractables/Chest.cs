using UnityEngine;

public class Chest : Inventory, IInteractable
{
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private int ID;
    public static int lastID;

    #region Get/Set
    public int  GetID()               { return ID;      }
    public bool GetIsOpen()           { return isOpen;  }
    #endregion

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastID++;
        ID = lastID;
    }

    public void CheckIfOpen()
    {
        if (isOpen) spriteRenderer.color = Color.red;
    }

    public bool CanInteract() { return !isOpen; }

    public void Interact()
    {
        if (!CanInteract()) return;
        isOpen = true;
        spriteRenderer.color = Color.red;
    }

    public InventoryData GetInventory() { return inventory; }
}

[System.Serializable]
public class ChestData
{
    public int ID;
    public bool IsOpen;
}