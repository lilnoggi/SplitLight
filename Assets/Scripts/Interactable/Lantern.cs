using UnityEngine;

public class Lantern : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Lantern picked up!");
        
        // Give lantern to player
        PlayerLantern playerLantern = FindObjectOfType<PlayerLantern>();
        if (playerLantern != null)
        {
            playerLantern.ObtainLantern();
        }

        // Optionally disable or destroy this lantern object
        gameObject.SetActive(false);
    }
}
