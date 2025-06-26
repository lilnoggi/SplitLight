using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    private IInteractable currentInteractable;
    private PlayerMovement playerMovement;

    public GameObject interactionPrompt;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            if (playerMovement != null)
                playerMovement.enabled = false;

            currentInteractable.Interact();
            HidePrompt();

            // Re-enable movement after a short delay (e.g. for animations or short interactions)
            Invoke(nameof(ReenableMovement), 1f); // adjust this duration as needed!
        }
    }

    private void ReenableMovement()
    {
        if (playerMovement != null)
            playerMovement.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            ShowPrompt();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IInteractable>() == currentInteractable)
        {
            currentInteractable = null;
            HidePrompt();
        }
    }

    private void ShowPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}
