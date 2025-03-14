using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCParent : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject namePanel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage;

    [SerializeField] Color nameColor;
    [SerializeField] Color placeColor;
    [SerializeField] Color itemColor;
    [SerializeField] Color buttonColor;

    public GameObject GetDialoguePanel() { return dialoguePanel; }
    public TMP_Text   GetDialogueText()  { return dialogueText;  }
    public GameObject GetNamePanel()     { return namePanel;     }
    public TMP_Text   GetNameText()      { return nameText;      }
    public Image      GetPortraitImage() { return portraitImage; }
}
