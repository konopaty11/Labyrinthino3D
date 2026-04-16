using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] int indexScene;

    public bool isLocked;

    public void Interact()
    {
        Debug.Log(isLocked);
        if (!isLocked)
            SceneManager.LoadScene(indexScene);
    }
}
