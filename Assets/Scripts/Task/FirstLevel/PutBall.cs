using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PutBall : TaskBase
{
    [SerializeField] GameObject password;

    void OnEnable()
    {
        PlayerController.PutItem += Complete;
    }

    void OnDisable()
    {
        PlayerController.PutItem -= Complete;
    }

    public override void Complete()
    {
        password.SetActive(true);
        base.Complete();
    }
}
