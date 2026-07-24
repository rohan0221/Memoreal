using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject settingsPanel;
    public GameObject creditsPanel;
    public Slider volumeSlider;

    void Start()
    {
        ShowMain();
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Bootstrap");
    }

    public void ShowMain()
    {
        mainPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}