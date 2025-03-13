using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] TabController tabController;
    [SerializeField] GameObject defaultSelection;
    [SerializeField] EventSystem eventSystem;
    void Start()
    {
        OnOpenCloseMenu(false);
        eventSystem.SetSelectedGameObject(defaultSelection);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            OnOpenCloseMenu(!menuCanvas.activeSelf);
        }
    }
    private void OnOpenCloseMenu(bool isOpen)
    {
        menuCanvas.SetActive(isOpen);

        if (isOpen) { Time.timeScale = 0f; }
        else        { Time.timeScale = 1f; }
    }
}
