public interface IInteractable
{
    void Interact();
    void Interact(InteractionDetector interactor);

    bool CanInteract();
}
