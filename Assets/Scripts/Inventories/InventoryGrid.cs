using TMPro;
using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] TextMeshProUGUI          infoText;
    [SerializeField] TextMeshProUGUI          descText;
    [Space]
    [Header("Item Usage")]
    [SerializeField] GameObject               itemGrid;
    [SerializeField] UseInventoryButton       use;
    [SerializeField] SwapInventoryButton      swap;
    [SerializeField] ThrowAwayInventoryButton throwAway;

    public void SetInfoText(string value)          { infoText.text = value;     }
    public void SetDescText(string value)          { descText.text = value;     }
    public GameObject GetItemGrid()                { return itemGrid;           }
    public UseInventoryButton GetUse()             { return use;                }
    public SwapInventoryButton GetSwap()           { return swap;               }
    public ThrowAwayInventoryButton GetThrowAway() { return throwAway;          }
    public void SetActiveItemGrid(bool value)      { itemGrid.SetActive(value); }
}
