using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    private NPCDialogue outcome;
    [SerializeField] private TextMeshProUGUI choiceText;
    public Button button;
    private NPC asker;

    public void InitializeButton()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChooseOption);
    }

    public void ChooseOption() { asker.StartNewDialogue(outcome); }

    public void SetOutcome(NPCDialogue _outcome) { outcome = _outcome; }

    public void SetChoiceText(string _choiceText) { choiceText.text = _choiceText; }
    public void SetAsker(NPC _asker) { asker = _asker; }
}