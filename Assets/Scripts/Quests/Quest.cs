using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    private bool isInProgress = false;
    private bool isCompleted = false;

    [Header("Quest Rewards")]
    [SerializeField] private InventoryData rewards; [Space]
    [Header("Quest Info")]
    [SerializeField] private new string name;
    [SerializeField] private string     description;
    [SerializeField] private string     rewardsDesc;
    [SerializeField] private string     asker;
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
    }

    public virtual void GiveQuestRewards(Inventory inventory)
    {
        if (rewards == null)
        {
            Debug.Log("Quest rewards null");
            return;
        }

        for (int i = 0; i < rewards.items.Count ; i++)
        {
            // If the quantities list does not match the rewards list
            if (rewards.quantities.Count != rewards.items.Count)
            {
                Debug.Log("Quest reward quantities does not match the amount of items. Distributing each item once");
                inventory.AddItem(rewards.items[i], 1);
                inventory.AddMoney(rewards.money);
                continue;
            }

            inventory.AddItem(rewards.items[i], rewards.quantities[i]);
        }
    }
}
