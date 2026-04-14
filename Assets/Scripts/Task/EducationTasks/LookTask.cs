using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTask : TaskBase
{
    [SerializeField] Transform camera;

    Quaternion _lastRotation;
    float _totalAngle;

    public override bool IsDetecting
    {
        get => base.IsDetecting;
        set
        {
            if (value)
            {
                _totalAngle = 0;
                _lastRotation = camera.rotation;
            }

            base.IsDetecting = value;
        }
    }

    void Start()
    {
        _lastRotation = camera.rotation;
    }

    void Update()
    {
        if (!IsDetecting)
            return;

        _totalAngle += Quaternion.Angle(_lastRotation, camera.rotation);
        Debug.Log($"{_lastRotation} -- {camera.rotation} -- {Quaternion.Angle(_lastRotation, camera.rotation)}");
        _lastRotation = camera.rotation;

        if (_totalAngle >= 180)
        {
            Complete();
            _totalAngle = 0;
        }
    }
}
