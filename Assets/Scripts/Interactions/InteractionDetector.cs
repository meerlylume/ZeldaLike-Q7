using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] GameObject interactionIcon;

    private IInteractable interactableInRange = null;
    [SerializeField] private GameObject    interactableGameobjectInRange;

    private void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started) 
        { 
            interactableInRange?.Interact();

            if (!interactableGameobjectInRange) return;

            interactableGameobjectInRange.TryGetComponent(out NPC npcComponent);
            if (npcComponent)
            {
                npcComponent.SetPlayerReference(transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactableGameobjectInRange = collision.gameObject;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactableGameobjectInRange = null;
            interactionIcon.SetActive(false);
        }
    }
}