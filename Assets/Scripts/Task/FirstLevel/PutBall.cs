using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PutBall : TaskBase
{
    [SerializeField] GameObject password;

    void OnEnable()
    {
        PlayerController.PutItem += OnPutBall;
    }

    void OnDisable()
    {
        PlayerController.PutItem -= OnPutBall;
    }

    void OnPutBall(ItemType type)
    {
        if (type != ItemType.Ball)
            return;
        
        password.SetActive(true);
        Complete();
    }
}
