using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    private bool          isInProgress = false;
    private bool          isCompleted = false;
    private InventoryData rewards;
    private new string    name;
    private string        description;
    private string        rewardsDesc;
    private string        asker;        //Lapsus had me forgetting how you call someone who asks you
                                        //something so until my brain gets repaired this is all we get.

    [Header("Dialogue SOs")]
    [SerializeField] NPCDialogue isInProgressDialogue; 
    [SerializeField] NPCDialogue rewardDialogue;
    [SerializeField] NPCDialogue isCompletedDialogue;

    #region Get/Set
    public bool          IsInProgress()          { return isInProgress;         }
    public bool          IsCompleted()           { return isCompleted;          }
    public InventoryData GetRewards()            { return rewards;              }
    public string        GetName()               { return name;                 }
    public string        GetDescription()        { return description;          }
    public string        GetRewardsDesc()        { return rewardsDesc;          }
    public string        GetAsker()              { return asker;                }
    public NPCDialogue GetIsInProgressDialogue() { return isInProgressDialogue; }
    public NPCDialogue GetRewardDialogue()       { return rewardDialogue;       }
    public NPCDialogue GetIsCompletedDialogue()  { return isCompletedDialogue;  }
    #endregion

    public virtual void AcceptQuest() { isInProgress = true; }

    public abstract void CheckIfCompleted();

    public virtual void CompleteQuest()
    {
        isInProgress = false;
        isCompleted = true;
        GiveQuestRewards();
    }

    public virtual void GiveQuestRewards()
    {
        //probably will need a ref to the player inventory
    }
}
