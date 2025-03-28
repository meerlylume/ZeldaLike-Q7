using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatGrid : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> texts;
    [SerializeField] Stats stats;

    public void RefreshStats()
    {
        texts[0].text = stats.maxHP.ToString();
        texts[1].text = stats.maxMana.ToString();
        texts[2].text = stats.attack.ToString();
        texts[3].text = stats.defence.ToString();
        texts[4].text = stats.creativity.ToString();
        texts[5].text = stats.recovery.ToString();
    }
}
