using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Button button;
    public UnityEvent Submit;

    private void Start() { button = GetComponent<Button>(); }
    public void SetText(string value) { text.text = value; }
    public Button GetButton() { return button; }
    public void OnClick() { Submit.Invoke(); }
}
