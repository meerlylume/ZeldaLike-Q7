using UnityEngine;

public class QuestNPC : NPC
{
    [SerializeField] private Quest quest;
    protected NPCDialogue trueRootDialogue;
    protected QuestTracker questTracker;

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
        // Check if rewards should be given
        if (quest.IsInProgress())
        {
            quest.CheckIfCompleted();
            if (quest.IsCompleted())
            {
                Debug.Log("reward");
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
            Debug.Log("completed");
            rootDialogueData   = quest.GetIsCompletedDialogue();
            branchDialogueData = rootDialogueData;
            return;
        }

        // Else they say the root dialogue
        //else base.StartDialogue();
        //Debug.Log("base.StartDialogue()");
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
        questTracker = _player.GetComponent<QuestTracker>();
    }
}
