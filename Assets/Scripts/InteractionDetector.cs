using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] GameObject interactionIcon;
    [SerializeField] PlayerInventory playerInventory;
    [SerializeField] PlayerFight playerFight;
    [SerializeField] GameObject currentCamera;

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
            DoInteract();
        }
    }

    public void DoInteract()
    {
        if (!canInteract) return;

        if (interactableGameobjects.Count == 0) return;

        int index = interactableGameobjects.Count - 1;

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
        if (item && item.CanInteract())
        {
            item.Interact();
            playerInventory.AddItem(item, 1);
            interactableGameobjects.RemoveAt(index);
            CheckInteractionIcon();
            return;
        }

        //CHEST
        interactableGameobjects[index].TryGetComponent(out Chest chest);
        if (chest && chest.CanInteract())
        {
            chest.Interact();
            InventoryData chestInventory = chest.GetInventory();
            for (int i = 0; chestInventory.items.Count > i; i++)
            {
                playerInventory.AddItem(chestInventory.items[i], chestInventory.quantities[i]);
                playerInventory.AddMoney(chestInventory.money);
            }
            interactableGameobjects.RemoveAt(index);
            CheckInteractionIcon();
            return;
        }

        //FULL HEALER
        interactableGameobjects[index].TryGetComponent(out FullHealer fullHealer);
        if (fullHealer && fullHealer.CanInteract())
        {
            playerFight.FullHeal();
            fullHealer.Interact();
            return;
        }

        interactableGameobjects[index].TryGetComponent(out Teleporter teleporter);
        if (teleporter && teleporter.CanInteract())
        {
            playerFight.transform.position = teleporter.GetDestination();

            currentCamera.SetActive(false);
            currentCamera = teleporter.GetNewCamera();
            currentCamera.SetActive(true);
        }

        interactableGameobjects[index].TryGetComponent(out IInteractable interactable);
        interactable?.Interact();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract()) 
            interactableGameobjects.Add(collision.gameObject);

        if (collision.TryGetComponent(out Item item) && item.CanInteract())
            item.HighlightSprite(true);

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
                    break;
                }
            }
        }

        if (collision.TryGetComponent(out Item item) && item.CanInteract()) 
            item.HighlightSprite(false);

        CheckInteractionIcon();
    }

    private void CheckInteractionIcon()
    {
        interactionIcon.SetActive(!(interactableGameobjects.Count == 0));
    }
}