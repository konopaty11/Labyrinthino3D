using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketballBackboard : MonoBehaviour
{
    public static event UnityAction Hit;

    bool _isHitted;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("ThrowingBall") && !_isHitted)
        {
            _isHitted = true;
            Hit?.Invoke();
        }
    }
}
