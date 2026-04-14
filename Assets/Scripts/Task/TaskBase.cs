using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskBase : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] TaskManager taskManager;

    public virtual bool IsDetecting { get; set; }

    public void Complete()
    {
        taskManager.OnTaskCompleted(index);
    }

}
