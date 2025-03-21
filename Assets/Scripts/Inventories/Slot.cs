using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("Slot Display")]
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image image; [Space]

    [Header("OnClick")]
    [SerializeField] GameObject grid;
    private InventoryGrid parent; [Space]

    [Header("ButtonPrefabs")]
    [SerializeField] UseInventoryButton use;
    [SerializeField] SwapInventoryButton swap;
    [SerializeField] ThrowAwayInventoryButton throwAway;
    //[SerializeField] CancelInventoryButton cancel;

    // Use, Swap, Throw Away, Cancel

    private PlayerInventory inventory;
    private Item item;
    private int quantity;

    #region Setters
    public void SetItem(Item value)                 { item      = value; }
    public void SetQuantity(int value)              { quantity  = value; }
    public void SetInventory(PlayerInventory value) { inventory = value; }
    #endregion

    private void Start()
    {
        //if (item) RefreshSlot();

        InventoryGrid parent = transform.parent.GetComponent<InventoryGrid>();
    }

    public void RefreshSlot()
    {
        if (quantity > 1)    quantityText.text = "x" + quantity;
        else                 quantityText.text = "";
        if (image && item.sprite) image.sprite = item.sprite;

        if (!parent) return;
        parent.SetDescText(item.description);
        parent.SetInfoText(item.information);
    }

    public void RemoveItem()
    {
        inventory.RemoveItem(item, quantity);
        RefreshSlot();
    }

    

    public void OnClick()
    {
        //Activate Layout Grid
        grid.SetActive(true);
        if (item.GetComponent<Consumable>() != null) use.gameObject.SetActive(true);
    }

    //handle usage ui

    //Find way to set the desc script
}
