using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Chest : Inventory, IInteractable
{
    protected InventoryData baseInventory;
    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //inventory copying no workie... why
        baseInventory  = inventory;
        inventory      = ScriptableObject.CreateInstance<InventoryData>();
        CopyBaseInventory();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void CopyBaseInventory()
    {
        inventory.items      = baseInventory.items;
        inventory.money      = baseInventory.money;
        inventory.quantities = baseInventory.quantities;
        inventory.name       = baseInventory.name;
    }

    public bool CanInteract() { return !isOpen; }

    public void Interact()
    {
        if (!CanInteract()) return;
        isOpen = true;
        spriteRenderer.color = Color.red;
    }

    public InventoryData GetInventory()
    {
        return inventory;
    }
}
