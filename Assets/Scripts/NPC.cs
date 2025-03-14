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
    GameObject  namePanel;
    TMP_Text    nameText;
    Image       portraitImage;

    private int lineIndex;
    private bool isTyping;
    private bool isDialogueActive;
    private string tagDetector;

    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {
        NPCParent _parent = transform.parent.GetComponent<NPCParent>();
        dialoguePanel = _parent.GetDialoguePanel();
        dialogueText  = _parent.GetDialogueText();
        namePanel     = _parent.GetNamePanel();
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

        if (isDialogueActive) { NextLine(); }

        else                  { StartDialogue(); }
    }

    void StartDialogue()
    {
        CheckPortraitPosition();

        isDialogueActive = true;
        lineIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        //Stop player momvement
        yield return new WaitForEndOfFrame();
        if (playerMovement) playerMovement.StopPlayerMovement();

        //Clear the dialogue text and start typing
        isTyping = true;
        dialogueText.SetText("");

        //Parse the text
        foreach(char letter in dialogueData.dialogueLines[lineIndex])
        {
            //Check for tags
            if (letter == '$')
            {
                tagDetector += letter;
                Debug.Log("Dialogue tag detected");
                yield return new WaitForEndOfFrame();
            }

            //If it's a tag tag, check what comes next
            if (tagDetector != "")
            {
                if (letter == ' ')
                {
                    tagDetector = "";
                    yield return new WaitForEndOfFrame();
                }

                tagDetector += letter;
            }

            //Add the letter and wait
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.talkingSpeed);
        }

        //Stop typing
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
    }

    public void SetPlayerReference(GameObject _player)
    {
        playerMovement = _player.GetComponent<PlayerMovement>();
    }

    void CheckPortraitPosition()
    {
        if (portraitImage.transform.localPosition.x > 0) //if the portrait is to the LEFT of the dialogue box
        {
            if (dialogueData.isPortraitOnTheRight) { SwapPortraitSide(); }
            return;
        }
        else                                          //if the portrait is to the RIGHT of the dialogue box
        {
            if (!dialogueData.isPortraitOnTheRight) { SwapPortraitSide(); }
            return;
        }
    }

    private void SwapPortraitSide()
    {
        dialogueText.transform.localPosition  = new Vector3(-dialogueText.transform.localPosition.x,  dialogueText.transform.localPosition.y);
        namePanel.transform.localPosition     = new Vector3(-namePanel.transform.localPosition.x,     namePanel.transform.localPosition.y);
        portraitImage.transform.localPosition = new Vector3(-portraitImage.transform.localPosition.x, portraitImage.transform.localPosition.y);
    }
}
