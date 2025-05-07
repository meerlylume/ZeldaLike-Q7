using UnityEngine;
using UnityEngine.UI;

public class GUIStatBar : MonoBehaviour
{
    [Header("For Mana bar")]
    [SerializeField] private bool ignoreHalfFill;
    [Header("Slider Color References")]
    [SerializeField] private Image fill;
    [SerializeField] private Image background;
    [Header("Full to 1/2 bar")]
    [SerializeField] private Color baseFill;
    [SerializeField] private Color baseBackground; 
    [Header("1/2 to 1/4 bar")]
    [SerializeField] private Color halfFill;
    [SerializeField] private Color halfBackground;
    [Header("1/4 to 0 bar")]
    [SerializeField] private Color fourthFill;
    [SerializeField] private Color fourthBackground;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void RefreshBar(float newMax, float currentValue)
    {
        slider.maxValue = newMax;
        slider.value    = currentValue;

        CheckColor();
    }

    private void CheckColor()
    {
        if (slider.value < slider.maxValue / 4)
        {
            fill.color = fourthFill;
            background.color = fourthBackground;
            return;
        }

        if (slider.value < slider.maxValue / 2 && !ignoreHalfFill)
        {
            fill.color       = halfFill;
            background.color = halfBackground;
            return;
        }

        fill.color = baseFill;
        background.color = baseBackground;
    }
}
