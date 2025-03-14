using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject defaultSelection;
    [SerializeField] EventSystem eventSystem;
    void Start()
    {
        OnOpenCloseMenu(false);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started) 
        {
            OnOpenCloseMenu(!menuCanvas.activeSelf);
            if (menuCanvas.activeSelf) eventSystem.SetSelectedGameObject(defaultSelection);
        }
    }
    private void OnOpenCloseMenu(bool isOpen)
    {
        menuCanvas.SetActive(isOpen);

        if (isOpen) { Time.timeScale = 0f; }
        else        { Time.timeScale = 1f; }
    }
}
