using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] List<TaskSerializable> tasks;
    [SerializeField] TextMeshProUGUI taskLabel;

    int _currentTaskIndex = -1;

    void Start()
    {
        ShowNextTask();
    }

    public void OnTaskCompleted(int index)
    {
        if (index != _currentTaskIndex)
            return;

        ShowNextTask();
    }

    void ShowNextTask()
    {
        if (_currentTaskIndex >= 0)
            tasks[_currentTaskIndex].task.IsDetecting = false;

        _currentTaskIndex++;

        if (_currentTaskIndex == tasks.Count)
        {
            return;
        }

        tasks[_currentTaskIndex].task.IsDetecting = true;
        taskLabel.text = tasks[_currentTaskIndex].description;
    }
}

[Serializable]
class TaskSerializable
{
    public TaskBase task;
    public string description;
}
