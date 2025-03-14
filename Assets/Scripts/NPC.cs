using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] NPCDialogue dialogueData;

    GameObject  dialoguePanel;
    TMP_Text    dialogueText;
    TMP_Text    nameText;
    Image       portraitImage;

    private int lineIndex;
    private bool isTyping;
    private bool isDialogueActive;

    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        NPCParent _parent = transform.parent.GetComponent<NPCParent>();
        dialoguePanel = _parent.GetDialoguePanel();
        dialogueText  = _parent.GetDialogueText();
        nameText      = _parent.GetNameText();
        portraitImage = _parent.GetPortraitImage();
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        if (!dialogueData)
        {
            Debug.Log("NO DIALOGUE DATA FOUND");
            return;
        }

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {

        isDialogueActive = true;
        lineIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);
        // * Disable Player Movement

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //FOR FUTURE CHARACTER COLOR: https://discussions.unity.com/t/fixed-change-color-of-individual-characters-in-textmeshpro-text-ui/880934/2

        yield return new WaitForEndOfFrame();
        Debug.Log("End of frame");
        if (playerMovement) playerMovement.StopPlayerMovement();

        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[lineIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.talkingSpeed);
        }

        isTyping = false;
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[lineIndex]);
            isTyping = false;
        }
        else if(++lineIndex < dialogueData.dialogueLines.Length) { StartCoroutine(TypeLine()); }
        else { EndDialogue(); }
    }

    public void EndDialogue()
    {
        if (playerMovement) playerMovement.SetCanMove(true);

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        // * Enable Player Movement
    }

    public void SetPlayerReference(GameObject _player)
    {
        playerMovement = _player.GetComponent<PlayerMovement>();
    }
}
