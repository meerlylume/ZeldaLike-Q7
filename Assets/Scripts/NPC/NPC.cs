using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] protected NPCDialogue rootDialogueData;
    protected NPCDialogue branchDialogueData;

    protected GameObject   dialoguePanel;
    protected TMP_Text     dialogueText;
    protected GameObject   namePanel;
    protected TMP_Text     nameText;
    protected Image        portraitImage;
    protected GameObject   choicesGrid;
    protected GameObject   choicePrefab;
    List<GameObject>     choiceButtons = new List<GameObject> { };

    protected int          lineIndex;
    protected bool         isTyping;
    protected bool         isDialogueActive;
    protected bool         isWaitingForChoice = false;
    protected string       tagDetector;

    protected NPCParent parent;
    protected PlayerMovement playerMovement;

    public void SetIsWaitingForChoice(bool value) { isWaitingForChoice = value; }

    protected virtual void Start()
    {
        branchDialogueData = rootDialogueData;

        parent = transform.parent.GetComponent<NPCParent>();

        dialoguePanel = parent.GetDialoguePanel();
        dialogueText  = parent.GetDialogueText();
        namePanel     = parent.GetNamePanel();
        nameText      = parent.GetNameText();
        portraitImage = parent.GetPortraitImage();
        choicesGrid   = parent.GetChoicesGrid();
        choicePrefab  = parent.GetChoicesPrefab();
    }

    public virtual void StartNewDialogue(NPCDialogue newData)
    {
        branchDialogueData = newData;

        if (choiceButtons == null) return;

        for (int i = 0; i < choiceButtons.Count; i++) { Destroy(choiceButtons[i]); }

        if (newData.name == "EndOfTree")
        {
            EndDialogue();
            return;
        }

        Interact();
    }

    public bool CanInteract() { return !isDialogueActive; }

    public virtual void Interact()
    {
        if (isWaitingForChoice) return;

        if (!branchDialogueData)
        {
            Debug.Log("NO DIALOGUE DATA FOUND");
            if (rootDialogueData) branchDialogueData = rootDialogueData;
            else return;
        }

        if (isDialogueActive) { NextLine(); }

        else { StartDialogue(); }
    }

    public virtual void StartDialogue()
    {
        CheckPortraitPosition();

        tagDetector      = "";
        isDialogueActive = true;
        lineIndex        = 0;

        nameText.SetText(branchDialogueData.name);
        portraitImage.sprite = branchDialogueData.npcPortrait;

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
        foreach(char letter in branchDialogueData.dialogueLines[lineIndex])
        {
            dialogueText.text += CheckingForTag(letter);

            //If there's no tag, add the character and wait
            if (tagDetector == "" && letter != '>')
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(branchDialogueData.talkingSpeed);
            }
        }

        //Stop typing
        isTyping = false;
    }

    protected void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            DisplayEntireLine();
            isTyping = false;
        }

        else if (++lineIndex < branchDialogueData.dialogueLines.Length) { StartCoroutine(TypeLine()); }

        else { EndDialogue(); }
    }

    protected void DisplayEntireLine()
    {
        dialogueText.SetText("");

        foreach (char letter in branchDialogueData.dialogueLines[lineIndex])
        {
            //Detect tag
            dialogueText.text += CheckingForTag(letter);

            //If there's no tag, add the letter and wait
            if (tagDetector == "" && letter != '>') { dialogueText.text += letter; }
        }
    }

    protected string CheckingForTag(char letter)
    {
        string returnedText = "";

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
                        returnedText += "<color=#" + parent.GetNameColor().ToHexString() + ">";
                        break;
                    case "</name>":
                        returnedText += "</color>";
                        break;

                    // ITEM
                    case "<item>":
                        returnedText += "<color=#" + parent.GetItemColor().ToHexString() + ">";
                        break;
                    case "</item>":
                        returnedText += "</color>";
                        break;

                    // PLACE
                    case "<place>":
                        returnedText += "<color=#" + parent.GetPlaceColor().ToHexString() + ">";
                        break;
                    case "</place>":
                        returnedText += "</color>";
                        break;

                    case "<reward>":
                    case "<rewards>":
                        GiveQuestRewards();
                        break;

                    // NO VALID TAG FOUND
                    default:
                        returnedText += tagDetector;
                        break;
                }

                tagDetector = "";
            }
        }

        return returnedText;
    }

    public virtual void GiveQuestRewards()
    {
        Debug.Log("NO REWARDS CAN BE GIVEN. NOT A QUEST NPC");
    }

    public virtual void EndDialogue()
    {
        if (branchDialogueData.dialogueChoices != null)
        {
            DisplayDialogueChoices();
            isWaitingForChoice = true;
        }

        else { if (playerMovement) playerMovement.EnablePlayerMovement(); }

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping         = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        branchDialogueData = rootDialogueData;
    }
    public void DisplayDialogueChoices()
    {
        for (int i = 0; i < branchDialogueData.dialogueChoices.choices.Count; i++)
        {
            GameObject choiceObject           = Instantiate(choicePrefab);
            choiceObject.transform.SetParent(choicesGrid.transform);
            choiceObject.transform.localScale = Vector3.one;

            ChoiceButton choiceButton = choiceObject.GetComponent<ChoiceButton>();
            choiceButton.SetChoiceText(branchDialogueData.dialogueChoices.choices[i]);
            choiceButton.SetChoiceText(ParseChoiceText(branchDialogueData.dialogueChoices.choices[i]));
            choiceButton.SetOutcome(branchDialogueData.dialogueChoices.outcomes[i]);
            choiceButton.SetAsker(this);

            choiceButton.InitializeButton();
            choiceButtons.Add(choiceButton.gameObject);
        }
    }

    protected string ParseChoiceText(string choiceText)
    {
        string resultText = string.Empty;
        
        foreach (char letter in choiceText)
        {
            //Detect tag
            resultText += CheckingForTag(letter);

            //If there's no tag, add the letter and wait
            if (tagDetector == "" && letter != '>') { resultText += letter; }
        }

        return resultText;
    }

    public virtual void SetPlayerReference(GameObject _player)
    {
        playerMovement = _player.GetComponent<PlayerMovement>();
    }

    protected void CheckPortraitPosition()
    {
        //if the portrait is to the LEFT of the dialogue box
        if (portraitImage.transform.localPosition.x > 0) 
        {
            //...but it's supposed to be on the RIGHT
            if (branchDialogueData.isPortraitOnTheRight)  { SwapPortraitSide(); }
            return;
        }

        //if the portrait is to the RIGHT of the dialogue box
        else
        {
            //...but it's supposed to be on the LEFT
            if (!branchDialogueData.isPortraitOnTheRight) { SwapPortraitSide(); }
            return;
        }
    }

    protected void SwapPortraitSide()
    {
        dialogueText.transform.localPosition  = new Vector3(-dialogueText.transform.localPosition.x, dialogueText.transform.localPosition.y);
        namePanel.transform.localPosition     = new Vector3(-namePanel.transform.localPosition.x, namePanel.transform.localPosition.y);
        portraitImage.transform.localPosition = new Vector3(-portraitImage.transform.localPosition.x, portraitImage.transform.localPosition.y);
    }
}
