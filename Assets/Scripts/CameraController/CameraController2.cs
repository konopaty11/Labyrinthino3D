using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float sensivity;
    [SerializeField] JoystickController look;
    [SerializeField] float height;
    [SerializeField] float speed;
    [SerializeField] float distance;
    [SerializeField] float minY;
    [SerializeField] float maxY;

    float _rotationX;
    float _rotationY;

    void Start()
    {
        _rotationY = player.eulerAngles.y;
    }

    private void LateUpdate()
    {
        Look();
        Follow();
    }


    void Look()
    {
        float inputX = look.Direction.x * sensivity * Time.deltaTime;
        float inputY = look.Direction.y * sensivity * Time.deltaTime;

        _rotationY += inputX;
        _rotationX -= inputY;

        _rotationX = Mathf.Clamp(_rotationX, minY, maxY);
    }

    void Follow()
    {
        Quaternion horizontal = Quaternion.Euler(0, _rotationY, 0);
        Vector3 offset = new(0f, height, 0f);

        Vector3 targetPosition = player.position + Vector3.down + horizontal * offset;
        transform.position = targetPosition;
        transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);

        player.rotation = Quaternion.Euler(player.eulerAngles.x, _rotationY, player.eulerAngles.z);
    }
}
