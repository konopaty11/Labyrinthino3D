using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitToBasketballBackboard : TaskBase
{
    [SerializeField] TextMeshProUGUI task;
    [SerializeField] GameObject password;

    int _countHits;

    void OnEnable()
    {
        BasketballBackboard.Hit += UpdateTask;
    }

    void OnDisable()
    {
        BasketballBackboard.Hit -= UpdateTask;
    }

    void UpdateTask()
    {
        _countHits++;
        if (_countHits == 3)
        {
            password.SetActive(true);
            Complete();
            return;
        }

        task.text = $"Киньте мяч и попадите им в баскетбольный щит ({_countHits}/3)";
    }
}
