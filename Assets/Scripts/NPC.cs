using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] NPCDialogue dialogueData;
    private NPCDialogue firstDialogue;

    private GameObject   dialoguePanel;
    private TMP_Text     dialogueText;
    private GameObject   namePanel;
    private TMP_Text     nameText;
    private Image        portraitImage;
    private GameObject   choicesGrid;
    private GameObject   choicePrefab;
    List<GameObject>     choiceButtons = new List<GameObject> { };

    private int        lineIndex;
    private bool       isTyping;
    private bool       isDialogueActive;
    private string     tagDetector;

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
        choicesGrid   = parent.GetChoicesGrid();
        choicePrefab  = parent.GetChoicesPrefab();

        firstDialogue = dialogueData;
    }

    public void StartNewDialogue(NPCDialogue newData)
    {
        dialogueData = newData;
        Interact();

        if (choiceButtons == null) return;

        for (int i = 0; i < choiceButtons.Count; i++) { Destroy(choiceButtons[i].gameObject); }
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

        tagDetector      = "";
        isDialogueActive = true;
        lineIndex        = 0;

        nameText.SetText(dialogueData.name);
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
                        dialogueText.text += "<color=#" + parent.GetItemColor().ToHexString() + ">";
                        break;
                    case "</item>":
                        dialogueText.text += "</color>";
                        break;

                    //PLACE
                    case "<place>":
                        dialogueText.text += "<color=#" + parent.GetPlaceColor().ToHexString() + ">";
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
        if (dialogueData.dialogueChoices != null) DisplayDialogueChoices();

        else { if (playerMovement) playerMovement.EnablePlayerMovement(); }

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping         = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }

    public void DisplayDialogueChoices()
    {
        for (int i = 0; i < dialogueData.dialogueChoices.choices.Count; i++)
        {
            GameObject choiceObject      = Instantiate(choicePrefab);
            choiceObject.transform.SetParent(choicesGrid.transform);

            ChoiceButton choiceButton    = choiceObject.GetComponent<ChoiceButton>();
            choiceButton.SetChoiceText(dialogueData.dialogueChoices.choices[i]);
            choiceButton.SetOutcome(dialogueData.dialogueChoices.outcomes[i]);
            choiceButton.SetAsker(this);

            choiceButton.InitializeButton();
            choiceButtons.Add(choiceButton.gameObject);
        }
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
