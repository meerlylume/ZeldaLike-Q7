using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCParent : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portraitImage;

    public GameObject GetDialoguePanel() { return dialoguePanel; }
    public TMP_Text   GetDialogueText()  { return dialogueText; }
    public TMP_Text   GetNameText()      { return nameText; }
    public Image      GetPortraitImage() { return portraitImage; }
}
