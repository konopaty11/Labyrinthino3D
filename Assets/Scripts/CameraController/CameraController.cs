using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _player;

    [SerializeField] float _sensitivityX = 120f;
    [SerializeField] float _sensitivityY = 120f;

    [SerializeField] float _minY = -80f;
    [SerializeField] float _maxY = 80f;

    [Header("Follow")]
    [SerializeField] float _cameraHeight = 1.7f;
    [SerializeField] float _followSpeed = 10f;
    [SerializeField] float _distanceFromPlayer = 5f;

    float _xRotation;
    float _yRotation;

    [Header("Input")]
    [SerializeField] JoystickController _lookJoystick;

    void Start()
    {
        _yRotation = _player.eulerAngles.y;
    }

    void LateUpdate()
    {
        Look();
        Follow();
    }

    void Look()
    {
        float inputX = _lookJoystick.Direction.x * _sensitivityX * Time.deltaTime;
        float inputY = _lookJoystick.Direction.y * _sensitivityY * Time.deltaTime;

        _yRotation += inputX;
        _xRotation -= inputY;

        _xRotation = Mathf.Clamp(_xRotation, _minY, _maxY);
    }

    void Follow()
    {
        Quaternion horizontalRotation = Quaternion.Euler(0f, _yRotation, 0f);

        Vector3 offset = new Vector3(0f, _cameraHeight);
        Vector3 targetPosition = _player.position + Vector3.down + horizontalRotation * offset;

        transform.position = targetPosition;

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0f); 
        _player.rotation = Quaternion.Euler(_player.eulerAngles.x, _yRotation, _player.eulerAngles.z);
    }
}