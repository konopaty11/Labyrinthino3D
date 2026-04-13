using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] LoadManager loadManager;
    [SerializeField] DialogManager dialogManager;

    public const string IsDialogPassed = "DialogPassed";

    void Start()
    {
        loadManager.StartLoad();
        StartDialog();

    }

    void StartDialog()
    {
        if (PlayerPrefs.GetInt(IsDialogPassed) == 0)
        {
            dialogManager.OpenWindow();
        }
    }
}
