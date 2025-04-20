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
    [Header("Inventory Page Dialogue Text")]
    [SerializeField] GameObject promptPanel;
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] GameObject choicesGrid;
    [SerializeField] GameObject choicePrefab;

    // SET
    public void SetInfoText(string value)     { infoText.text = value;       }
    public void SetDescText(string value)     { descText.text = value;       }
    public void SetActiveItemGrid(bool value) { itemGrid.SetActive(value);   }

    // GET
    public UseInventoryButton       GetUse()          { return use;          }
    public SwapInventoryButton      GetSwap()         { return swap;         }
    public GameObject               GetItemGrid()     { return itemGrid;     }
    public ThrowAwayInventoryButton GetThrowAway()    { return throwAway;    }
    public TextMeshProUGUI          GetPromptText()   { return promptText;   }
    public GameObject               GetChoicesGrid()  { return choicesGrid;  }
    public GameObject               GetPromptPanel()  { return promptPanel;  }
    public GameObject               GetChoicePrefab() { return choicePrefab; }
}
