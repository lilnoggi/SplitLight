using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    public GameObject lanternLight;
    private bool hasLantern = false;
    private bool lanternOn = false;

    void Start()
    {
        if (lanternLight != null)
            lanternLight.SetActive(false);
    }

    void Update()
    {
        if (hasLantern && Input.GetKeyDown(KeyCode.F))
        {
            ToggleLantern();
        }
    }

    public void ObtainLantern()
    {
        hasLantern = true;
        Debug.Log("Player now has the lantern.");
        // show a UI message "Press F to use the lantern"
    }

    void ToggleLantern()
    {
        lanternOn = !lanternOn;
        if (lanternLight != null)
            lanternLight.SetActive(lanternOn);
    }
}
