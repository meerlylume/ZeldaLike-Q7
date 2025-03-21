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
            interactableInRange?.Interact();

            if (!interactableGameobjectInRange) return;

            //NPC
            interactableGameobjectInRange.TryGetComponent(out NPC npc);
            if (npc)
            {
                npc.SetPlayerReference(transform.parent.gameObject);
                return;
            }

            //ITEM
            interactableGameobjectInRange.TryGetComponent(out Item item);
            if (item)
            {
                playerInventory.AddItem(item, 1);
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