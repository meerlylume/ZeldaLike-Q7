using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTransitionInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] string scene;
    private bool isOpen = true;

    public bool CanInteract() { return isOpen;                 }

    public void Interact()    { SceneManager.LoadScene(scene); }

    public void Interact(InteractionDetector interactor)
    {
    }
}
