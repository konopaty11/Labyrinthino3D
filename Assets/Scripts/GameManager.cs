using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// main managerof the game
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] LoadManager loadManager;
    [SerializeField] DialogManager dialogManager;
    [SerializeField] MainMenuManager mainMenuManager;

    public const string IsDialogPassed = "DialogPassed";
    public const string SoundActive = "SoundActive";

    /// <summary>
    /// start game
    /// </summary>
    void Start()
    {
        loadManager.StartLoad();
        StartDialog();

    }

    /// <summary>
    /// begin the dialog
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
    /// on dialog ended
    /// </summary>
    public void DialogEnded()
    {
        mainMenuManager.OpenMainMenu();
    }
}
