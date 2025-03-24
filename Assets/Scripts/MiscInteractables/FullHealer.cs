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
}
