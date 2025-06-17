using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    private Animator animator;
    private bool hasLantern = false;
    private bool lanternEquipped = false;

    public GameObject equipPromptUI;

    public float maxHealth = 100f;
    private float currentHealth;

    public LanternHealthUI lanternUI;

    public AudioClip pickupSound;
    public AudioClip equipSound;
    public AudioClip unequipSound;

    private AudioSource audioSource;

    public AudioSource idleLoopSource;

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

        animator.SetBool("HasLantern", hasLantern);
    }

    public void PickUpLantern()
    {
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
        if (!hasLantern) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (lanternUI != null)
        lanternUI.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            // Handle lantern burnout or player death here
            Debug.Log("Lantern depleted! You're vulnerable!");
        }
    }
}
