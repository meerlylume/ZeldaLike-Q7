using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] GameObject interactionIcon;
    [SerializeField] PlayerInventory playerInventory;

    private List <GameObject> interactableGameobjects = new List<GameObject>();
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
            int index = interactableGameobjects.Count - 1;

            if (interactableGameobjects[index] == null) return;

            //NPC
            interactableGameobjects[index].TryGetComponent(out NPC npc);
            if (npc)
            {
                npc.Interact();
                npc.SetPlayerReference(transform.parent.gameObject);
                CheckInteractionIcon();
                return;
            }

            //ITEM
            interactableGameobjects[index].TryGetComponent(out Item item);
            if (item)
            {
                if (!item.CanInteract()) return;
                item.Interact();
                playerInventory.AddItem(item, 1);
                interactableGameobjects.RemoveAt(index);
                CheckInteractionIcon();
                return;
            }

            //CHEST
            interactableGameobjects[index].TryGetComponent(out Chest chest);
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
                CheckInteractionIcon();
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract()) 
            interactableGameobjects.Add(collision.gameObject);

        CheckInteractionIcon();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            if (!interactable.CanInteract()) return;
            
            for (int i = 0; i < interactableGameobjects.Count; i++)
            {
                if (interactableGameobjects[i] == collision.gameObject)
                {
                    interactableGameobjects.RemoveAt(i);
                    CheckInteractionIcon();
                    return;
                }
            }
        }

        CheckInteractionIcon();
    }

    private void CheckInteractionIcon()
    {
        interactionIcon.SetActive(!(interactableGameobjects.Count == 0));
    }
}