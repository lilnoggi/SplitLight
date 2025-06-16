using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    private Animator animator;
    private bool hasLantern = false;
    private bool lanternEquipped = false;

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
        // play a sound or animation
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
