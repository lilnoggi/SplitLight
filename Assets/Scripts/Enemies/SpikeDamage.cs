using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    public float damageAmount = 20f;       // Customize this per spike type
    public float damageCooldown = 1f;      // Prevents multiple hits per second

    private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            PlayerLantern playerLantern = other.GetComponent<PlayerLantern>();
            if (playerLantern != null)
            {
                playerLantern.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
