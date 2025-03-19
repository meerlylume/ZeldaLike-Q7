using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private NPCDialogue rootDialogueData;
    private NPCDialogue branchDialogueData;

    private GameObject   dialoguePanel;
    private TMP_Text     dialogueText;
    private GameObject   namePanel;
    private TMP_Text     nameText;
    private Image        portraitImage;
    private GameObject   choicesGrid;
    private GameObject   choicePrefab;
    List<GameObject>     choiceButtons = new List<GameObject> { };

    private int          lineIndex;
    private bool         isTyping;
    private bool         isDialogueActive;
    private bool         isWaitingForChoice = false;
    private string       tagDetector;

    private NPCParent parent;
    private PlayerMovement playerMovement;

    public void SetIsWaitingForChoice(bool value) { isWaitingForChoice = value; }

    private void Start()
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

    public void StartNewDialogue(NPCDialogue newData)
    {
        branchDialogueData = newData;

        if (choiceButtons == null) return;

        for (int i = 0; i < choiceButtons.Count; i++) { Destroy(choiceButtons[i].gameObject); }

        if (newData.name == "EndOfTree")
        {
            EndDialogue();
            return;
        }

        Interact();
    }

    public bool CanInteract() { return !isDialogueActive; }

    public void Interact()
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

    void StartDialogue()
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

    void NextLine()
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

    void DisplayEntireLine()
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

    string CheckingForTag(char letter)
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

    public void EndDialogue()
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
            GameObject choiceObject      = Instantiate(choicePrefab);
            choiceObject.transform.SetParent(choicesGrid.transform);

            ChoiceButton choiceButton    = choiceObject.GetComponent<ChoiceButton>();
            choiceButton.SetChoiceText(branchDialogueData.dialogueChoices.choices[i]);
            choiceButton.SetChoiceText(ParseChoiceText(branchDialogueData.dialogueChoices.choices[i]));
            choiceButton.SetOutcome(branchDialogueData.dialogueChoices.outcomes[i]);
            choiceButton.SetAsker(this);

            choiceButton.InitializeButton();
            choiceButtons.Add(choiceButton.gameObject);
        }
    }

    string ParseChoiceText(string choiceText)
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

    public void SetPlayerReference(GameObject _player)
    {
        playerMovement = _player.GetComponent<PlayerMovement>();
    }

    void CheckPortraitPosition()
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

    private void SwapPortraitSide()
    {
        dialogueText.transform.localPosition  = new Vector3(-dialogueText.transform.localPosition.x, dialogueText.transform.localPosition.y);
        namePanel.transform.localPosition     = new Vector3(-namePanel.transform.localPosition.x, namePanel.transform.localPosition.y);
        portraitImage.transform.localPosition = new Vector3(-portraitImage.transform.localPosition.x, portraitImage.transform.localPosition.y);
    }
}
