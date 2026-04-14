using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// основнйо менеджер игры
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] LoadManager loadManager;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] MainMenuManager mainMenuManager;

    public const string IsDialogPassed = "DialogPassed";
    public const string SoundActive = "SoundActive";

    /// <summary>
    /// старт
    /// </summary>
    void Start()
    {
        loadManager.StartLoad();
        StartDialog();

    }

    /// <summary>
    /// начало диалога
    /// </summary>
    void StartDialog()
    {
        if (PlayerPrefs.GetInt(IsDialogPassed) == 0)
        {
            dialogManager.OpenWindow();
        }
        else
        {
            DialogEnded();
        }
    }

    /// <summary>
    /// окончание диалога
    /// </summary>
    public void DialogEnded()
    {
        mainMenuManager.OpenMainMenu();
    }
}
