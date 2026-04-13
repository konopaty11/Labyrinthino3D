using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogCanvas;
    [SerializeField] GameObject player;
    [SerializeField] Color playerColor;
    [SerializeField] GameObject robot;
    [SerializeField] Color robotColor;
    [SerializeField] TextMeshProUGUI author;
    [SerializeField] TextMeshProUGUI content;
    [SerializeField] DialogConfig dialogConfig;

    int _currentMessageIndex = 0;

    public void OpenWindow()
    {
        dialogCanvas.SetActive(true);
        ShowNextMessage();
    }

    public void CloseWindow()
    {
        PlayerPrefs.SetInt(GameManager.IsDialogPassed, 1);
        dialogCanvas.SetActive(false);
    }

    public void Skip()
    {
        CloseWindow();
    }

    public void Next()
    {
        ShowNextMessage();
    }

    void ShowNextMessage()
    {
        if (_currentMessageIndex == dialogConfig.logs.Count)
        {
            CloseWindow();
            return;
        }

        LogSerializable log = dialogConfig.logs[_currentMessageIndex];
        if (log.author == Author.Player)
        {
            player.SetActive(true);
            robot.SetActive(false);

            author.color = playerColor;
        }
        else if (log.author == Author.Robot)
        {
            player.SetActive(false);
            robot.SetActive(true);

            author.color = robotColor;
        }

        author.text = log.name;
        content.text = log.message;

        _currentMessageIndex++;
    }
}
