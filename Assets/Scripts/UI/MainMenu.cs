using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject newGameButton;
    [SerializeField] GameObject deleteSaveButton;

    private void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (GameSaver.Instance != null)
        {
            if (GameSaver.Instance.HasSaveFile())
            {
                continueButton.SetActive(true);
                newGameButton.SetActive(false);
                deleteSaveButton.SetActive(true);
            }
            else
            {
                continueButton.SetActive(false);
                newGameButton.SetActive(true);
                deleteSaveButton.SetActive(false);
            }
        }
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnDeleteSave()
    {
        RefreshButtons();
    }

    public void OnCredits()
    {
        Application.OpenURL("https://meerlylume.itch.io/");
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNewGame()
    {
        SceneManager.LoadScene("PrologueScene");
    }
}
