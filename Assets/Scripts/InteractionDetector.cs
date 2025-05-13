using System;
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
    int index;

    private void Start() { interactionIcon.SetActive(false); }

    private void CheckInteractionIcon() { interactionIcon.SetActive(!(interactableGameobjects.Count == 0)); }

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
        if (interactableGameobjects.Count == 0) return;

        index = interactableGameobjects.Count - 1;

        interactableGameobjects[index].TryGetComponent(out IInteractable interactable);
        interactable?.Interact(this);

        CheckInteractionIcon();
    }

    public void ItemInteract(Item item) 
    { 
        playerInventory.AddItem(item, 1); 
        interactableGameobjects.RemoveAt(index);
    }

    public void TeleporterInteract(Teleporter teleporter)
    {
        playerFight.transform.position = teleporter.GetDestination();

        currentCamera.SetActive(false);
        currentCamera = teleporter.GetNewCamera();
        currentCamera.SetActive(true);
    }

    public void ChestInteract(Chest chest)
    {
        InventoryData chestInventory = chest.GetInventory();
        for (int i = 0; chestInventory.items.Count > i; i++)
        {
            playerInventory.AddItem(chestInventory.items[i], chestInventory.quantities[i]);
            playerInventory.AddMoney(chestInventory.money);
        }
        interactableGameobjects.RemoveAt(index);
    }

    public void FullHealerInteract(FullHealer fullHealer) { playerFight.FullHeal(); }

    public void NPCInteract(NPC npc)
    {
        npc.Interact();
        npc.SetPlayerReference(transform.parent.gameObject);
    }

    #region Collisions
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
                if (interactableGameobjects[i] != collision.gameObject) continue;
                interactableGameobjects.RemoveAt(i);
                CheckInteractionIcon();
                break;
            }
        }

        if (collision.TryGetComponent(out Item item) && item.CanInteract()) 
            item.HighlightSprite(false);

        CheckInteractionIcon();
    }
    #endregion
}