using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> texts;

    int _currentIndex;

    public void SetNextNumber(char number)
    {
        texts[_currentIndex].text = number.ToString();
    }
}
