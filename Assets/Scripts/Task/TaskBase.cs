using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] TaskManager taskManager;

    public virtual bool IsDetecting { get; set; }

    public virtual void Complete()
    {
        taskManager.OnTaskCompleted(index);
    }

}
