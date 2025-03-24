using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Slot Display")]
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image image; [Space]

    private InventoryGrid parent;

    private PlayerInventory inventory;
    private Item item;
    private int quantity;

    // Use, Swap, Throw Away, Cancel

    #region Getters & Setters
    public void SetItem(Item value)                 { item      = value;           }
    public void SetQuantity(int value)              { quantity  = value;           }
    public void SetInventory(PlayerInventory value) { inventory = value;           }
    private GameObject GetItemGrid()                { return parent.GetItemGrid(); }
    #endregion

    private void Start()
    {
        parent = transform.parent.GetComponent<InventoryGrid>();
    }

    public void RefreshSlot()
    {
        // Quantity
        if (quantity > 1)    quantityText.text = "x" + quantity;
        else                 quantityText.text = "";

        // Sprite
        if (image && item.sprite) image.sprite = item.sprite;

        RefreshDescAndInfo();
    }

    public void RefreshDescAndInfo()
    {
        if (!parent) return;
        parent.SetDescText(item.description);
        parent.SetInfoText(item.information);
    }

    public void RefreshThrowAway()
    {
        inventory.RemoveItem(item, quantity);
        ThrowAwayInventoryButton throwAway = parent.GetThrowAway();
        throwAway.SetInventory(inventory);
        throwAway.SetItem(item);
        throwAway.SetQuantity(quantity);
        RefreshSlot();
    }

    public void OnClick()
    {
        //  Move Grid to right position and activate it
        parent.SetActiveItemGrid(true);
        GetItemGrid().transform.position = transform.position;

        RefreshThrowAway();

        // If it's consumable, activate the Use button
        if (item.GetComponent<Consumable>() != null)
        {
            UseInventoryButton use = parent.GetUse();
            use.gameObject.SetActive(true);
            use.SetInventory(inventory);
            use.SetItem(item);
        }
    }

    //handle usage ui
}
