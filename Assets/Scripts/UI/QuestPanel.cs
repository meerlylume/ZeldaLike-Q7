using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDesc;
    [SerializeField] TextMeshProUGUI rewardsDesc;

    public void SetQuestName(string value)   { questName.text    = value;}
    public void SetQuestDesc(string value)   { questDesc.text    = value;}
    public void SetRewardsDesc(string value) {  rewardsDesc.text = value;}
}
