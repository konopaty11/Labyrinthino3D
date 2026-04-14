using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] int indexScene;

    public void Interact()
    {
        SceneManager.LoadScene(indexScene);
    }
}
