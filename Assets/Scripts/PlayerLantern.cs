using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLantern : MonoBehaviour
{
    private Animator animator;
    private bool hasLantern = false;
    private bool lanternEquipped = false;

    public GameObject equipPromptUI;

    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;

    public Transform respawnPoint;
    public float respawnDelay = 2f;

    public LanternHealthUI lanternUI;

    public AudioClip pickupSound;
    public AudioClip equipSound;
    public AudioClip unequipSound;

    private AudioSource audioSource;

    public AudioSource idleLoopSource;

    [Header("Sound Effects")]
    public AudioClip deathSound;
    public AudioClip deathLanternSound;
    public AudioClip takeDamageSound;

    public Tilemap hiddenTilemap;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) // press H to test
        {
            TakeDamage(10f);
        }

        if (hasLantern && Input.GetKeyDown(KeyCode.F))
        {
            lanternEquipped = !lanternEquipped;
            animator.SetBool("LanternEquipped", lanternEquipped);

            PlaySound(lanternEquipped ? equipSound : unequipSound);

            if (idleLoopSource != null)
            {
                if (lanternEquipped && !idleLoopSource.isPlaying)
                    idleLoopSource.Play();
                else if (!lanternEquipped && idleLoopSource.isPlaying)
                    idleLoopSource.Stop();
            }
        }

        hiddenTilemap.gameObject.SetActive(lanternEquipped);

        animator.SetBool("HasLantern", hasLantern);
    }

    public void PickUpLantern()
    {
        animator.SetTrigger("Interact");
        hasLantern = true;
        currentHealth = maxHealth;

        PlaySound(pickupSound);
        if (lanternUI != null)
        {
            lanternUI.Show();
            lanternUI.UpdateHealth(currentHealth, maxHealth);
        }

        if (equipPromptUI != null)
        {
            equipPromptUI.SetActive(true);
            Invoke(nameof(HideEquipPrompt), 3f);
        }
    }

    private void HideEquipPrompt()
    {
        if (equipPromptUI != null)
        {
            equipPromptUI.SetActive(false);
        }
    }

    public bool IsLanternEquipped()
    {
        return hasLantern && lanternEquipped;
    }

    public bool HasLantern()
    {
        return hasLantern;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public void TakeDamage(float amount)
    {
        if (!hasLantern || isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (lanternUI != null)
            lanternUI.UpdateHealth(currentHealth, maxHealth);
        
        if (takeDamageSound != null)
            PlaySound(takeDamageSound);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("You Died");

        if (idleLoopSource != null && idleLoopSource.isPlaying)
            idleLoopSource.Stop();

        if (lanternEquipped)
        {
            animator.SetTrigger("DieWithLantern");
            PlaySound(deathLanternSound);
        }
        else
        {
            animator.SetTrigger("DieWithoutLantern");
            PlaySound(deathSound);
        }
        // Disable input or other movement here (optional, depends on setup)
        GetComponent<PlayerMovement>().enabled = false;

        // respawn coroutine
        StartCoroutine(RespawnCoroutine());
    }

    private System.Collections.IEnumerator RespawnCoroutine()
{
    yield return new WaitForSeconds(respawnDelay);

    // Reset player position
    if (respawnPoint != null)
        transform.position = respawnPoint.position;

    // Reset health
    currentHealth = maxHealth;
    if (lanternUI != null)
        lanternUI.UpdateHealth(currentHealth, maxHealth);

    // Re-enable movement
    GetComponent<PlayerMovement>().enabled = true;

    // Reset death state
    isDead = false;

    // Optionally play idle animation
    animator.SetTrigger("Respawn"); // make a "Respawn" trigger in your animator that transitions to Idle

    Debug.Log("Player respawned");
}

}
