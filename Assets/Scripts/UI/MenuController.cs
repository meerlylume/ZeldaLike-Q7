using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject menuDefaultSelection;
    [SerializeField] GameObject settingsDefaultSelection;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] PlayerFight playerFight;

    void Start()
    {
        OnOpenCloseMenu(false);
        playerFight.ManaRanOutEvent.AddListener(ForceClose);
    }

    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.started) OpenSelected(menuDefaultSelection);
    }

    private void ForceClose()
    {
        OnOpenCloseMenu(false);
    }

    private void OnOpenCloseMenu(bool isOpen)
    {
        if (isOpen && !playerFight.DoDefaultMenu())
        {
            if (!playerFight.HasMana()) return;

            playerFight.OnMenuOpen();
        }

        if (!isOpen) playerFight.OnMenuClose();
        
        menuCanvas.SetActive(isOpen);

        if (isOpen) { Time.timeScale = 0f; }
        else        { Time.timeScale = 1f; }
    }

    public void ToggleSettings(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OpenSelected(menuDefaultSelection);
            OpenSelected(settingsDefaultSelection);
        }
    }

    private void OpenSelected(GameObject selected)
    {
        OnOpenCloseMenu(!menuCanvas.activeSelf);
        if (menuCanvas.activeSelf) eventSystem.SetSelectedGameObject(selected);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("DevTent");
    }
}
