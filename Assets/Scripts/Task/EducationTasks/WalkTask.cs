using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTask : TaskBase
{
    [SerializeField] Transform player;

    Vector3 _lastPosition;
    float _totalDistance;

    public override bool IsDetecting 
    { 
        get => base.IsDetecting;
        set
        {
            _totalDistance = 0;
            _lastPosition = player.position;

            base.IsDetecting = value;
        }
    }

    void Start()
    {
        _lastPosition = GetPlayerPosition();
    }

    void Update()
    {
        if (!IsDetecting)
            return;

        Vector3 playerPosition = GetPlayerPosition();
        _totalDistance += Vector3.Distance(_lastPosition, playerPosition);
        _lastPosition = playerPosition;

        if (_totalDistance >= 4)
        {
            Complete();
            _totalDistance = 0;
        }
    }

    Vector3 GetPlayerPosition()
    {
        Vector3 playerPosition = player.position;
        playerPosition.y = 0;

        return playerPosition;
    }
}
