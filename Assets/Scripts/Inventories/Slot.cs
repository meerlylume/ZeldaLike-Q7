using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] Image image;

    private PlayerInventory inventory;
    private Item item;
    private int quantity;

    #region Setters
    public void SetItem(Item value)                 { item = value;      }
    public void SetQuantity(int value)              { quantity = value;  }
    public void SetInventory(PlayerInventory value) { inventory = value; }
    #endregion

    private void Start()
    {
        if (item) RefreshSlot();
    }

    public void RefreshSlot()
    {
        if (quantity > 1)    quantityText.text = "x" + quantity;
        else                 quantityText.text = "";
        if (image && item.sprite) image.sprite = item.sprite;
    }

    public void RemoveItem()
    {
        inventory.RemoveItem(item, quantity);
        RefreshSlot();
    }

    public void ConsumeItem()
    {
        Consumable consumable = item.GetComponent<Consumable>();
        if (consumable) inventory.ConsumeItem(consumable);
    }

    //handle usage ui

    //Find way to set the desc script
}
