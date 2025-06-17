// UISoundPlayer.cs
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Selectable))]
public class UISoundPlayer : MonoBehaviour,
    IPointerEnterHandler, ISelectHandler, ISubmitHandler, IPointerClickHandler
{
    public enum UISoundType { Toggle, Select, StartGame }
    public UISoundType soundTypeOnClick = UISoundType.Select;

    public AudioClip hoverClip;       // assign UI_CursorToggle.wav
    public AudioClip clickClip;       // assign UI_CursorSelect.wav or UI_TitleScreen_StartGameSelect.wav
    public string gameSceneName;      // only needed for StartGame button
    public AudioSource audioSource;

    // Hover / keyboard navigation
    public void OnPointerEnter(PointerEventData eventData) => PlayHover();
    public void OnSelect(BaseEventData eventData)         => PlayHover();

    // Click / submit
    public void OnPointerClick(PointerEventData eventData) => PlayClick();
    public void OnSubmit(BaseEventData eventData)          => PlayClick();

    void PlayHover()
    {
        if (hoverClip != null && audioSource != null)
            audioSource.PlayOneShot(hoverClip);
    }

    void PlayClick()
    {
        if (clickClip != null && audioSource != null)
        {
            if (soundTypeOnClick == UISoundType.StartGame)
            {
                StartCoroutine(LoadGameWithSFX(clickClip.length));
            }
            else
            {
                audioSource.PlayOneShot(clickClip);
            }
        }
    }

    IEnumerator LoadGameWithSFX(float delay)
    {
        audioSource.PlayOneShot(clickClip);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(gameSceneName);
    }
}
