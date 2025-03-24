using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject progressGrid;
    [SerializeField] GameObject inProgressPrefab; 
    [Space]
    [SerializeField] GameObject completedGrid;
    [SerializeField] GameObject completedPrefab;

    private List<Quest> quests = new List<Quest>();
    public List<Quest> GetQuests() { return quests; }

    private void Start()
    {
        RefreshAllPanels();
    }

    public void OnQuestPanelOpen()
    {
        RefreshAllPanels();
    }

    public void RefreshAllPanels()
    {
        MurderTheirChildren(progressGrid);
        MurderTheirChildren(completedGrid);

        BuildLists();
    }

    private void BuildLists()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (!quests[i]) continue;

            if (quests[i].IsDone())
            {
                GameObject completedObject          = Instantiate(completedPrefab);
                completedPrefab.transform.SetParent(completedGrid.transform);

                CompletedQuestPanel completedScript = completedObject.GetComponent<CompletedQuestPanel>();

                completedScript.SetQuestDesc(quests[i].GetDescription());
                completedScript.SetQuestName(quests[i].GetName());
            }
            else
            {
                GameObject progressObject = Instantiate(inProgressPrefab);
                completedPrefab.transform.SetParent(progressGrid.transform);

                QuestPanel progressScript = progressObject.GetComponent<QuestPanel>();

                progressScript.SetQuestDesc(quests[i].GetDescription());
                progressScript.SetQuestName(quests[i].GetName());
                progressScript.SetRewardsDesc(quests[i].GetRewardsDesc());
            }
        }
    }

    private void MurderTheirChildren(GameObject panel)
    {
        for (int i = 0; i < panel.transform.childCount; i++) { Destroy(panel.transform.GetChild(i)); }
    }
}
