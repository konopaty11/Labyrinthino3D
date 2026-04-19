using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBall : MonoBehaviour
{
    [SerializeField] new SphereCollider collider;
    [SerializeField] Rigidbody rg;

    public void Throw()
    {
        collider.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrier"))
        {
            Barrier barrier = other.GetComponent<Barrier>();
            if (barrier.BallColorType != BallColorType.Yellow)
                Destroy(gameObject);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            rg.velocity = Vector3.zero;
            rg.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        Debug.Log(rg.velocity.magnitude);
    }
}
