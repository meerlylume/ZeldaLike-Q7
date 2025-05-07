using UnityEngine;
using UnityEngine.UI;

public class GUIStatBar : MonoBehaviour
{
    [Header("For Mana bar")]
    [SerializeField] private bool ignoreHalfFill; [Space]
    [Header("Slider Color References")]
    [SerializeField] private GameObject fillGameobject;
                     private Image fill;
    [SerializeField] private Image background; [Space]
    [Header("Full bar")]
    [SerializeField] private Color c_fullFill;
    [Header("Full to 1/2 bar")]
    [SerializeField] private Color c_baseFill;
    [SerializeField] private Color c_baseBackground; 
    [Header("1/2 to 1/4 bar")]
    [SerializeField] private Color c_halfFill;
    [SerializeField] private Color c_halfBackground;
    [Header("1/4 to 0 bar")]
    [SerializeField] private Color c_fourthFill;
    [SerializeField] private Color c_fourthBackground;

    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = fillGameobject.GetComponent<Image>();
    }

    public void RefreshBar(float newMax, float currentValue)
    {
        slider.maxValue = newMax;
        slider.value    = currentValue;

        if (currentValue <= 0f) fillGameobject.SetActive(false);
        else                    fillGameobject.SetActive(true);

        CheckColor();
    }

    private void CheckColor()
    {
        if (slider.value == slider.maxValue)
        {
            fill.color = c_fullFill;
            return;
        }

        if (slider.value < slider.maxValue / 4)
        {
            fill.color = c_fourthFill;
            background.color = c_fourthBackground;
            return;
        }

        if (slider.value < slider.maxValue / 2 && !ignoreHalfFill)
        {
            fill.color       = c_halfFill;
            background.color = c_halfBackground;
            return;
        }

        fill.color = c_baseFill;
        background.color = c_baseBackground;
    }
}
