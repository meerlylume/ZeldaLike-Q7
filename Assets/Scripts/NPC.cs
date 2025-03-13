using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] NPCDialogue dialogueData;
    [SerializeField] GameObject  dialoguePanel;
    [SerializeField] TMP_Text    dialogueText;
    [SerializeField] TMP_Text    nameText;
    [SerializeField] Image       portraitImage;

    private int lineIndex;
    private bool isTyping;
    private bool isDialogueTalking;
}
