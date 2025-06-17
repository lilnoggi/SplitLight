using UnityEngine;
using UnityEngine.UI;

public class LanternHealthUI : MonoBehaviour
{
    public GameObject lanternUI;
    public Image fillImage;

    private void Start()
    {
        lanternUI.SetActive(false);
        fillImage.fillAmount = 1f; 
    }

    public void Show()
    {
        lanternUI.SetActive(true);
    }

    public void UpdateHealth(float current, float max)
    {
        fillImage.fillAmount = Mathf.Clamp01(current / max);
    }
}
