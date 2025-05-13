using UnityEngine;

public class Teleporter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject newCamera;

    public Vector3  GetDestination() { return destination.transform.position; }
    public GameObject GetNewCamera() { return newCamera; }

    public bool CanInteract()
    {
        return destination != null;
    }

    public void Interact()
    {

    }

    public void Interact(InteractionDetector interactor)
    {
        if (!CanInteract()) return;

        interactor.TeleporterInteract(this);
    }
}
