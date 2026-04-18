using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VanishingPlatform : MonoBehaviour
{
    [SerializeField] float activeStateTime = 6f;
    [SerializeField] float inactiveStateTime = 3f;
    [SerializeField] Collider playerCollider;

    float _timer;
    bool _active;
    Renderer _renderer;
    Collider _collider;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();

        _timer = activeStateTime;
        _active = true;
    }

    void Update()
    {
        if (_timer < 0)
        {
            if (_active)
            {
                _renderer.enabled = false;
                _collider.enabled = false;
                _timer = inactiveStateTime;
            }
            else
            {
                if (IsPlayerInside())
                    return;

                _renderer.enabled = true;
                _collider.enabled = true;
                _timer = activeStateTime;
            }

            _active = !_active;
        }

        _timer -= Time.deltaTime;
    }

    bool IsPlayerInside()
    {
        return _collider.bounds.Intersects(playerCollider.bounds);
    }

}
