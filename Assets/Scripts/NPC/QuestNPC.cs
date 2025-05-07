using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] private Quest quest;
    protected NPCDialogue trueRootDialogue;
    protected QuestTracker questTracker;
    protected PlayerInventory playerInventory;
    public Quest GetQuest() { return quest; }
    public void SetQuest(Quest value) { quest = value; }

    protected override void Start()
    {
        base.Start();
        trueRootDialogue = rootDialogueData;

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

    //handle reward giving, idfk how ngl...

    public override void StartDialogue()
    {
        CheckQuestDialogue();

        base.StartDialogue();
    }

    public override void EndDialogue()
    {
        if (branchDialogueData.questTrigger)
        {
            quest.AcceptQuest();
            questTracker.TrackQuest(quest);
            CheckQuestDialogue();
        }

        base.EndDialogue();
    }

    public override void SetPlayerReference(GameObject _player)
    {
        base.SetPlayerReference(_player);
        questTracker    = _player.GetComponent<QuestTracker>();
        playerInventory = _player.GetComponent<PlayerInventory>();
    }

    public override void GiveQuestRewards() { quest.GiveQuestRewards(playerInventory); }
}
