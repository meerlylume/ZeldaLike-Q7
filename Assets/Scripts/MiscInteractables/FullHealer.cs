using UnityEngine;

public class FullHealer : MonoBehaviour, IInteractable
{
    bool canBeUsed = true;

    public bool CanInteract()
    {
        return canBeUsed;
    }

    public void Interact()
    {
        //feedback
    }

    public void Interact(InteractionDetector interactor)
    {
        if (!CanInteract()) return;
        EventManager.Instance.RespawnEnemiesEvent.Invoke();
        interactor.FullHealerInteract(this);
    }
}
