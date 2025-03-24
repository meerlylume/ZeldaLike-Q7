using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    private bool          inProgress = false;
    private bool          isDone = false;
    private InventoryData rewards;
    private new string    name;
    private string        description;
    private string        rewardsDesc;
    private string        asker;        //Lapsus had me forgetting how you call someone who asks you
                                        //something so until my brain gets repaired this is all we get.
    #region Get/Set
    public bool          InProgress()     { return inProgress;   }
    public bool          IsDone()         { return isDone;       }
    public InventoryData GetRewards()     { return rewards;      }
    public string        GetName()        { return name;         }
    public string        GetDescription() { return description;  }
    public string        GetRewardsDesc() {  return rewardsDesc; }
    public string        GetAsker()       { return asker;        }
    #endregion

    public virtual void AcceptQuest() { inProgress = true; }

    public abstract void CheckIfCompleted();

    public virtual void CompleteQuest()
    {
        inProgress = false;
        isDone = true;
        GiveQuestRewards();
    }

    public virtual void GiveQuestRewards()
    {
        //probably will need a ref to the player inventory
    }
}
