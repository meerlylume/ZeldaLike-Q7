using TMPro;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI descText;

    public void SetInfoText(string value) { infoText.text = value; }
    public void SetDescText(string value) { descText.text = value; }
}
