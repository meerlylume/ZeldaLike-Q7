using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] protected NPCDialogue rootDialogueData;
    protected SpriteRenderer overworldSprite;
    protected NPCDialogue branchDialogueData;

    protected GameObject   dialoguePanel;
    protected TMP_Text     dialogueText;
    protected GameObject   namePanel;
    protected TMP_Text     nameText;
    protected Image        portraitImage;
    protected GameObject   choicesGrid;
    protected GameObject   choicePrefab;
    protected GameObject   nextArrow;
    List<GameObject>       choiceButtons = new List<GameObject> { };

    protected int          lineIndex;
    protected bool         isTyping;
    protected bool         isDialogueActive;
    protected bool         isWaitingForChoice = false;
    protected string       tagDetector;

    protected NPCParent parent;
    protected PlayerController playerController;
    protected PlayerInventory playerInventory;

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
        nextArrow     = parent.GetNextArrow();
        choicesGrid   = parent.GetChoicesGrid();
        choicePrefab  = parent.GetChoicesPrefab();

        overworldSprite = GetComponent<SpriteRenderer>();
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

        if (!branchDialogueData.identity) Debug.LogWarning("Branch Dialogue Data has no Identity");

        nameText.SetText(branchDialogueData.identity.name);
        if (branchDialogueData.identity.portrait) portraitImage.sprite = branchDialogueData.identity.portrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

protected IEnumerator TypeLine()
    {
        //Stop player movement
        yield return new WaitForEndOfFrame();
        if (playerController)
        {
            playerController.FreezePlayerMovement();
            playerController.SetCanAttack(false);
        }

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
                    case "<names>":
                        returnedText += "<color=#" + parent.GetNameColor().ToHexString() + ">";
                        break;

                    // ITEM
                    case "<item>":
                    case "<items>":
                        returnedText += "<color=#" + parent.GetItemColor().ToHexString() + ">";
                        break;

                    // PLACE
                    case "<place>":
                    case "<places>":
                        returnedText += "<color=#" + parent.GetPlaceColor().ToHexString() + ">";
                        break;

                    // MECHANIC
                    case "<mechanic>":
                    case "<mechanics>":
                        returnedText += "<color=#" + parent.GetMechanicColor().ToHexString() + ">";
                        break;
                    case "</name>":
                    case "</names>":
                    case "</item>":
                    case "</items>":
                    case "</place>":
                    case "</places>":
                    case "</mechanic>":
                    case "</mechanics>":
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
        if (branchDialogueData.giveItems)
        {
            Debug.Log("GIVE ITEMS");
            playerInventory.AddMoney(branchDialogueData.itemsToGive.money);
            
            for (int i = 0; i < branchDialogueData.itemsToGive.items.Count; i++)
            {
                if (branchDialogueData.itemsToGive.items.Count != branchDialogueData.itemsToGive.quantities.Count)
                    Debug.LogError("WRONG INVENTORY DATA");
                playerInventory.AddItem(branchDialogueData.itemsToGive.items[i], branchDialogueData.itemsToGive.quantities[i]);
            }
        }

        if (branchDialogueData.dialogueChoices != null)
        {
            DisplayDialogueChoices();
            isWaitingForChoice = true;
        }

        else 
        { 
            if (playerController)
            {
                playerController.UnfreezePlayerMovement();
                playerController.SetCanAttack(true);
            }
        }

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping         = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        if (branchDialogueData.nextDialogue != null)
        {
            StartNewDialogue(branchDialogueData.nextDialogue);
            return;
        }

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

    public virtual void SetPlayerReference(GameObject player)
    {
        playerController  = player.GetComponent<PlayerController>();
        playerInventory = player.GetComponent<PlayerInventory>();
        if (player.transform.position.x < transform.position.x) overworldSprite.flipX = true;
        else overworldSprite.flipX = false;
    }

    protected virtual void CheckPortraitPosition()
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

    protected virtual void SwapPortraitSide()
    {
        dialogueText.transform.localPosition  = new Vector3(-dialogueText.transform.localPosition.x, dialogueText.transform.localPosition.y);
        namePanel.transform.localPosition     = new Vector3(-namePanel.transform.localPosition.x, namePanel.transform.localPosition.y);
        portraitImage.transform.localPosition = new Vector3(-portraitImage.transform.localPosition.x, portraitImage.transform.localPosition.y);
        nextArrow.transform.eulerAngles       = new Vector3(0, nextArrow.transform.eulerAngles.y - 180, 270);
    }

    public void Interact(InteractionDetector interactor) { interactor.NPCInteract(this); }
}
