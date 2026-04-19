using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBallsGetter : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject ballPrefab;

    float _time = 2f;

    void Update()
    {
        if (player.ThrowingBall != null)
        {
            _time = 0;
            return;
        }

        _time += Time.deltaTime;

        if (_time >= 2f)
            SpawnBall();
    }

    void SpawnBall()
    {
        GameObject ballObject = Instantiate(ballPrefab, playerCamera);
        player.ThrowingBall = ballObject;
    }
}
