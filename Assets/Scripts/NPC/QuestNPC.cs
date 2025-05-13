using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] private Quest quest;
    protected NPCDialogue trueRootDialogue;
    protected QuestTracker questTracker;
    private bool doCheck = true;
    public Quest GetQuest() { return quest; }
    public void SetQuest(Quest value) { quest = value; }

    protected override void Start()
    {
        base.Start();
        trueRootDialogue = rootDialogueData;
        doCheck = true;

        CheckQuestDialogue();
    }

    public void CheckQuestDialogue()
    {
        if (!quest.IsInProgress() && !quest.IsCompleted()) return;

        if (quest.IsInProgress())
        {
            quest.CheckIfCompleted();
            // Check if rewards should be given
            if (quest.IsCompleted())
            {
                rootDialogueData   = quest.GetRewardDialogue();
                branchDialogueData = rootDialogueData;
                return;
            }
            else
            {
                rootDialogueData   = quest.GetIsInProgressDialogue();
                branchDialogueData = rootDialogueData;
                return;
            }
        }

        // Check if the NPC should say the completed dialogue
        if (quest.IsCompleted())
        {
            rootDialogueData   = quest.GetIsCompletedDialogue();
            branchDialogueData = rootDialogueData;
            return;
        }
    }

    public override void StartDialogue()
    {
        //if (rootDialogueData != quest.GetIsInProgressDialogue() && rootDialogueData != quest.GetIsCompletedDialogue()) CheckQuestDialogue(); //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

        if (doCheck)
        {
            CheckQuestDialogue();
            doCheck = false;
        }

        base.StartDialogue();
    }

    public override void EndDialogue()
    {
        if (branchDialogueData.giveItems)
        {
            playerInventory.AddMoney(branchDialogueData.itemsToGive.money);

            for (int i = 0; i < branchDialogueData.itemsToGive.items.Count; i++)
            {
                if (branchDialogueData.itemsToGive.items.Count != branchDialogueData.itemsToGive.quantities.Count)
                    Debug.LogError("WRONG INVENTORY DATA");
                playerInventory.AddItem(branchDialogueData.itemsToGive.items[i], branchDialogueData.itemsToGive.quantities[i]);
            }
        }

        if (branchDialogueData.questTrigger)
        {
            quest.AcceptQuest();
            questTracker.TrackQuest(quest);
        }

        if (branchDialogueData.dialogueChoices != null)
        {
            DisplayDialogueChoices();
            isWaitingForChoice = true;
            doCheck = false;
        }

        else 
        { 
            if (playerMovement) playerMovement.UnfreezePlayerMovement();
            doCheck = true;
        }

        StopAllCoroutines();
        isDialogueActive = false;
        isTyping = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

        if (branchDialogueData.nextDialogue != null)
        {
            doCheck = false;
            StartNewDialogue(branchDialogueData.nextDialogue);
            return;
        }

        branchDialogueData = rootDialogueData;
    }

    public override void SetPlayerReference(GameObject _player)
    {
        base.SetPlayerReference(_player);
        questTracker = _player.GetComponent<QuestTracker>();
    }

    public override void GiveQuestRewards() { quest.GiveQuestRewards(playerInventory); }
}
