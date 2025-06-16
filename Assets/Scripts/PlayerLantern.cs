using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    private Animator animator;
    private bool hasLantern = false;
    private bool lanternEquipped = false;

    public GameObject equipPromptUI;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hasLantern && Input.GetKeyDown(KeyCode.F))
        {
            lanternEquipped = !lanternEquipped;
            animator.SetBool("LanternEquipped", lanternEquipped);
        }

        animator.SetBool("HasLantern", hasLantern);
    }

    public void PickUpLantern()
    {
        hasLantern = true;

        if (equipPromptUI != null)
        {
            equipPromptUI.SetActive(true);
            Invoke(nameof(HideEquipPrompt), 3f);
        }
        // play a sound or animation
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
}
