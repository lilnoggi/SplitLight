using UnityEngine;

public class Lantern : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLantern playerLantern = other.GetComponent<PlayerLantern>();
            if (playerLantern != null)
            {
                playerLantern.PickUpLantern();
                Destroy(gameObject); // removes lantern from floor
            }
        }
    }
}
