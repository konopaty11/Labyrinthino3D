using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector3 firstPosition;
    [SerializeField] Vector3 secondPosition;

    [SerializeField] float speed = 1f;
    [SerializeField] float waitTime = 3f;

    public Vector3 PlatformOffset { get; private set; }
    public Vector3 Velocity { get; private set; }

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

            PlatformOffset = Vector3.zero;
            Velocity = Vector3.zero;
            return;
        }

        Vector3 oldPos = transform.position;

        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            _target,
            speed * Time.deltaTime
        );

        Vector3 delta = transform.position - oldPos;

        PlatformOffset = delta;
        Velocity = delta / Time.deltaTime;

        if (Vector3.Distance(transform.localPosition, _target) < 0.01f)
        {
            _waiting = true;
            _waitTimer = waitTime;
        }
    }
}
