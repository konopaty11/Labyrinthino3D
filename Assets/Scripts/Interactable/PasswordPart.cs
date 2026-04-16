using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordPart : MonoBehaviour, IInteractable
{
    public char symbol;

    public void Interact()
    {
        Destroy(gameObject);
    }
}
