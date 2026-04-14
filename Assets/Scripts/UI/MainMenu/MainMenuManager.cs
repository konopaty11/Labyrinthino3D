using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// менеджер основного окна
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] SettingsManager settingsManager;
    [SerializeField] List<GameObject> buttons;

    public void HandleVisibleButtons(bool active)
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(active);
        }
    }

    public void OpenMainMenu()
    {
        mainMenuCanvas.SetActive(true);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
