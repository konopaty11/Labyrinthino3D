using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 firstPosition;
    [SerializeField] Vector3 secondPosition;
    [SerializeField] PlayerController player;

    [SerializeField] float speed = 1f;
    [SerializeField] float waitTime = 3f;

    Vector3 _target;
    float _waitTimer;
    bool _waiting;

    void Start()
    {
        _target = secondPosition;
    }

    void Update()
    {
        if (_waiting)
        {
            _waitTimer -= Time.deltaTime;
            if (_waitTimer <= 0)
            {
                _waiting = false;
                _target = _target == firstPosition? secondPosition : firstPosition;
            }
            return;
        }

        Vector3 oldPos = transform.position;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            _target,
            speed * Time.deltaTime
        );

        Vector3 delta = transform.position - oldPos;

        if (IsPlayerOnPlatform())
        {
            player.PlatformOffset = delta;
        }

        if (Vector3.Distance(transform.localPosition, _target) < 0.01f)
        {
            _waiting = true;
            _waitTimer = waitTime;
        }
    }

    bool IsPlayerOnPlatform()
    {
        return Physics.Raycast
            (
                player.transform.position,
                Vector3.down,
                out RaycastHit hit,
                1.2f
            ) && hit.transform == transform;
    }
}
