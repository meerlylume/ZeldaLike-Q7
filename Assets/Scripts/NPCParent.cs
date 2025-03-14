using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCParent : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject namePanel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage; [Space]

    [Header("Text Colors")]
    [SerializeField] Color defaultColor;
    [SerializeField] Color nameColor;
    [SerializeField] Color placeColor;
    [SerializeField] Color itemColor;

    public GameObject GetDialoguePanel() { return dialoguePanel; }
    public TMP_Text   GetDialogueText()  { return dialogueText;  }
    public GameObject GetNamePanel()     { return namePanel;     }
    public TMP_Text   GetNameText()      { return nameText;      }
    public Image      GetPortraitImage() { return portraitImage; }
    public Color      GetDefaultColor()  { return defaultColor;  }
    public Color      GetNameColor()     { return nameColor;     }
    public Color      GetPlaceColor()    { return placeColor;    }
    public Color      GetItemColor()     { return itemColor;     }
}
