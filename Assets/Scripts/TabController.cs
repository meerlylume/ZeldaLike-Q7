using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] private Image[] tabImages;
    [SerializeField] private GameObject[] tabPages;
    [SerializeField] private int currentTabIndex = 0;
    [SerializeField] private int defaultTabIndex = 0;
    private Vector3 yOnSelected = new Vector3(0, 5, 0);

    private void Start()
    {
        ResetTabs();
    }

    public void ResetTabs()
    {
        for (int i = 0; i < tabImages.Length; i++)
        {
            tabImages[i].color = Color.gray;
            tabPages[i].SetActive(false);
        }

        _SetActiveTab(defaultTabIndex);

        //if (defaultTabIndex == 0) { _SetActiveTab(tabPages.Length - 1); }
        //else                         _SetActiveTab(defaultTabIndex);

    }

    //Idea: Make it possible to change the default tab in the options

    public void ActivateTab(int tabNo)
    {
        if (tabPages[currentTabIndex].activeSelf) _SetInactiveTab(currentTabIndex);

        if (!tabPages[tabNo].activeSelf)          _SetActiveTab(tabNo);
    }

    public void OnPressTab(InputAction.CallbackContext context) //Tab as in tabulation, the keyboard button. I must write this down because I am stoopid and will forget
    {
        if (context.started) 
        {
            ActivateTab(currentTabIndex + 1);
        }
    }

    private void _SetActiveTab(int tabNo)
    {
        tabPages[tabNo].SetActive(true);
        tabImages[tabNo].color = Color.white;
        //tabImages[tabNo].transform.position += yOnSelected;

        currentTabIndex = tabNo;
    }

    private void _SetInactiveTab(int tabNo)
    {
        tabPages[tabNo].SetActive(false);
        tabImages[tabNo].color = Color.gray;
        //tabImages[tabNo].transform.position -= yOnSelected;
    }
}
