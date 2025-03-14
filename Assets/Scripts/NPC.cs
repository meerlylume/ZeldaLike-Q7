using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] NPCDialogue dialogueData;

    GameObject dialoguePanel;
    TMP_Text dialogueText;
    GameObject namePanel;
    TMP_Text nameText;
    Image portraitImage;

    private int lineIndex;
    private bool isTyping;
    private bool isDialogueActive;
    private string tagDetector;

    private NPCParent parent;
    private PlayerMovement playerMovement;

    private void Start()
    {
        parent = transform.parent.GetComponent<NPCParent>();

        dialoguePanel = parent.GetDialoguePanel();
        dialogueText  = parent.GetDialogueText();
        namePanel     = parent.GetNamePanel();
        nameText      = parent.GetNameText();
        portraitImage = parent.GetPortraitImage();
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

        else { StartDialogue(); }
    }

    void StartDialogue()
    {
        CheckPortraitPosition();
        tagDetector = "";

        isDialogueActive = true;
        lineIndex        = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

IEnumerator TypeLine()
    {
        //Stop player movement
        yield return new WaitForEndOfFrame();
        if (playerMovement) playerMovement.DisablePlayerMovement();

        //Clear the dialogue text and start typing
        isTyping = true;
        dialogueText.SetText("");

        //Parse the text
        foreach(char letter in dialogueData.dialogueLines[lineIndex])
        {
            CheckingForTag(letter);

            //If there's no tag, add the character and wait
            if (tagDetector == "" && letter != '>')
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(dialogueData.talkingSpeed);
            }
        }

        //Stop typing
        isTyping = false;
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            DisplayEntireLine();
            //dialogueText.SetText(dialogueData.dialogueLines[lineIndex]);
            isTyping = false;
        }

        else if (++lineIndex < dialogueData.dialogueLines.Length) { StartCoroutine(TypeLine()); }

        else { EndDialogue(); }
    }

    void DisplayEntireLine()
    {
        dialogueText.SetText("");

        foreach (char letter in dialogueData.dialogueLines[lineIndex])
        {
            //Detect tag
            CheckingForTag(letter);

            //If there's no tag, add the letter and wait
            if (tagDetector == "" && letter != '>') { dialogueText.text += letter; }
        }
    }

    void CheckingForTag(char letter)
    {
        if (letter == '<') { tagDetector += letter; }

        //If tag detected, don't wait after adding the character to the dialogue
        else if (tagDetector != "")
        {
            tagDetector += letter;

            if (letter == '>')
            {
                switch (tagDetector)
                {
                    // NAME
                    case "<name>":
                        dialogueText.text += "<color=#" + parent.GetNameColor().ToHexString() + ">";
                        break;
                    case "</name>":
                        dialogueText.text += "</color>";
                        break;

                    //ITEM
                    case "<item>":

                        break;
                    case "</item>":
                        dialogueText.text += "</color>";
                        break;

                    //PLACE
                    case "<place>":

                        break;
                    case "</place>":
                        dialogueText.text += "</color>";
                        break;
                    default:
                        dialogueText.text += tagDetector;
                        break;
                }

                tagDetector = "";
            }
        }
    }

    public void EndDialogue()
    {
        if (playerMovement) playerMovement.EnablePlayerMovement();

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping         = false;
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
            if (dialogueData.isPortraitOnTheRight)  { SwapPortraitSide(); }
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
        dialogueText.transform.localPosition  = new Vector3(-dialogueText.transform.localPosition.x, dialogueText.transform.localPosition.y);
        namePanel.transform.localPosition     = new Vector3(-namePanel.transform.localPosition.x, namePanel.transform.localPosition.y);
        portraitImage.transform.localPosition = new Vector3(-portraitImage.transform.localPosition.x, portraitImage.transform.localPosition.y);
    }
}
