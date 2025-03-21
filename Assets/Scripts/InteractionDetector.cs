using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] GameObject interactionIcon;
    [SerializeField] PlayerInventory playerInventory;

    private IInteractable interactableInRange = null;
    private GameObject    interactableGameobjectInRange;
    private bool canInteract = true;

    private void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!canInteract) return;

        if (context.started) 
        { 
            if (!interactableGameobjectInRange) return;

            //NPC
            interactableGameobjectInRange.TryGetComponent(out NPC npc);
            if (npc)
            {
                npc.Interact();
                npc.SetPlayerReference(transform.parent.gameObject);
                interactionIcon.SetActive(false);
                return;
            }

            //ITEM
            interactableGameobjectInRange.TryGetComponent(out Item item);
            if (item)
            {
                if (!item.CanInteract()) return;
                item.Interact();
                playerInventory.AddItem(item, 1);
                interactionIcon.SetActive(false);
                return;
            }

            //CHEST
            interactableGameobjectInRange.TryGetComponent(out Chest chest);
            if (chest)
            {
                if (!chest.CanInteract()) return;
                chest.Interact();
                InventoryData chestInventory = chest.GetInventory();
                for (int i = 0; chestInventory.items.Count > i; i++)
                {
                    playerInventory.AddItem(chestInventory.items[i], chestInventory.quantities[i]);
                    playerInventory.AddMoney(chestInventory.money);
                }
                interactionIcon.SetActive(false);
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
            if (interactableInRange.CanInteract()) interactionIcon.SetActive(true);
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