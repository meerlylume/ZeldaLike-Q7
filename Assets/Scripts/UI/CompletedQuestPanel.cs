using TMPro;
using UnityEngine;

public class CompletedQuestPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDesc;
    public void SetQuestName(string value) { questName.text = value; }
    public void SetQuestDesc(string value) { questDesc.text = value; }
}
