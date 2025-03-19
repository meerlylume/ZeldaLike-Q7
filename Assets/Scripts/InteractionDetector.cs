using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] GameObject interactionIcon;
    [SerializeField] PlayerInventory playerInventory;

    private IInteractable interactableInRange = null;
    private GameObject    interactableGameobjectInRange;

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

            //NPC
            interactableGameobjectInRange.TryGetComponent(out NPC npcComponent);
            if (npcComponent)
            {
                npcComponent.SetPlayerReference(transform.parent.gameObject);
                return;
            }

            //CHEST
            interactableGameobjectInRange.TryGetComponent(out Chest chest);
            if (chest)
            {
                InventoryData chestInventory = chest.GetInventory();
                for (int i = 0; chestInventory.items.Count > i; i++)
                {
                    playerInventory.AddItem(chestInventory.items[i], chestInventory.quantities[i]);
                    playerInventory.AddMoney(chestInventory.money);
                }
                chest.EmptyEverything();
                return;
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