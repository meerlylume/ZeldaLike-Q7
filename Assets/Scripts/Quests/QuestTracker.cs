using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class QuestTracker : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject progressGrid;
    [SerializeField] GameObject isInProgressPrefab; 
    [Space]
    [SerializeField] GameObject completedGrid;
    [SerializeField] GameObject completedPrefab;

    private List<GameObject> allPanels = new List<GameObject>();

    private List<Quest> quests = new List<Quest>();
    public List<Quest> GetQuests() { return quests; }

    public void TrackQuest(Quest quest) { quests.Add(quest); }

    private void Start() { OnQuestPanelOpen(); }

    public void OnQuestPanelOpen()
    {
        BuildLists();
    }

    private void BuildLists()
    {
        if (allPanels.Count == quests.Count) return;

        if (allPanels.Count > quests.Count)
        {
            Debug.Log("ERROR: allPanels.Count > quests.Count");
            return;
        }

        for (int i = allPanels.Count; i < quests.Count; i++)
        {
            if (quests[i].IsCompleted())
            {
                GameObject completedObject = Instantiate(completedPrefab);
                completedObject.transform.SetParent(completedGrid.transform);

                CompletedQuestPanel completedScript = completedObject.GetComponent<CompletedQuestPanel>();

                completedScript.SetQuestDesc(quests[i].GetDescription());
                completedScript.SetQuestName(quests[i].GetName());

                completedObject.transform.localScale = new Vector3(1, 1, 1);

                allPanels.Add(completedObject);
            }
            else
            {
                GameObject progressObject = Instantiate(isInProgressPrefab);
                progressObject.transform.SetParent(progressGrid.transform);

                QuestPanel progressScript = progressObject.GetComponent<QuestPanel>();

                progressScript.SetQuestDesc(quests[i].GetDescription());
                progressScript.SetQuestName(quests[i].GetName());
                progressScript.SetRewardsDesc(quests[i].GetRewardsDesc());

                progressObject.transform.localScale = new Vector3(1, 1, 1);

                allPanels.Add(progressObject);
            }
        }
    }
}
