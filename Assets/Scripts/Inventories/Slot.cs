using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Slot Display")]
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image image; [Space]

    private InventoryGrid parent;

    private PlayerInventory playerInventory;
    private InventoryData   inventoryData;
    private int itemIndex;
    //private Item item;
    //private int quantity;

    #region Getters & Setters
    public void SetItemIndex(int value)               { itemIndex       = value;     }
    public void SetInventory(PlayerInventory value)   { playerInventory = value;     }
    public void SetInventoryData(InventoryData value) { inventoryData   = value;     }
    private GameObject GetItemGrid()                  { return parent.GetItemGrid(); }
    #endregion

    private void Start()
    {
        parent = transform.parent.GetComponent<InventoryGrid>();
    }

    public void RefreshSlot()
    {
        if (inventoryData.quantities.Count < itemIndex)
        {
            Debug.Log("inventoryData.quantities.Count < itemIndex");
            return;
        }

        // Quantity
        if (inventoryData.quantities[itemIndex] > 1)    quantityText.text = "x" + inventoryData.quantities[itemIndex];
        else                 quantityText.text = "";

        // Sprite
        if (image && inventoryData.items[itemIndex].sprite) image.sprite = inventoryData.items[itemIndex].sprite;

        RefreshDescAndInfo();
    }

    public void RefreshDescAndInfo()
    {
        if (!parent) return;
        parent.SetDescText(inventoryData.items[itemIndex].description);
        parent.SetInfoText(inventoryData.items[itemIndex].information);
    }

    public void RefreshThrowAway()
    {
        ThrowAwayInventoryButton throwAway = parent.GetThrowAway();
        throwAway.SetInventory(playerInventory);
        throwAway.SetItemIndex(itemIndex);
        throwAway.SetQuantity(inventoryData.quantities[itemIndex]);
        RefreshSlot();
    }

    public void OnClick()
    {
        //  Move Grid to right position and activate it
        parent.SetActiveItemGrid(true);
        GetItemGrid().transform.position = transform.position;

        RefreshThrowAway();

        // If it's consumable, activate the Use button
        if (inventoryData.items[itemIndex].GetComponent<Consumable>() != null)
        {
            UseInventoryButton use = parent.GetUse();
            use.gameObject.SetActive(true);
            use.SetInventory(playerInventory);
            use.SetItemIndex(itemIndex);
        }
    }
}
