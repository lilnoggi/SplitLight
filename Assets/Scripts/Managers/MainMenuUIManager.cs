using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject levelSelectPanel;
    public GameObject creditsPanel;

    public void OnPlayButton()
    {
        // Load game scene here (replace with actual scene name)
        SceneManager.LoadScene("TutorialScene");
    }

    public void OnOptionsButton()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnLevelSelectButton()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void OnCreditsButton()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void OnBackButton()
    {
        optionsPanel.SetActive(false);
        levelSelectPanel.SetActive(false);
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
        Debug.Log("Bye Bye...");
    }
}
