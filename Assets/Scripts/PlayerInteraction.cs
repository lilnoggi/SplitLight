using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private IInteractable currentInteractable;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact();
            //Interact animation
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            // show interact UI prompt
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() == currentInteractable)
        {
            currentInteractable = null;
            // hide prompt
    }
  }
}
