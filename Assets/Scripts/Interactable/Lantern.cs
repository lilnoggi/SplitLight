using UnityEngine;

public class Lantern : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerLantern playerLantern = player.GetComponent<PlayerLantern>();

        if (playerLantern != null)
        {
            playerLantern.PickUpLantern();
            Destroy(gameObject); // Remove lantern from scene
        }
    }
}
