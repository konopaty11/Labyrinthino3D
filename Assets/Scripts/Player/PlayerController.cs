using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1.7f;
    [SerializeField] float _accelerationTime = 0.5f;
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] float _jumpForce = 5f;

    [Header("Step settings")]
    [SerializeField] float _stepHeight = 0.4f;
    [SerializeField] float _stepCheckDistance = 0.5f;

    CharacterController _controller;
    Vector3 _velocity;
    Vector3 _currentMove;

    float _currentSpeed;
    float _time;

    [Header("Input")]
    [SerializeField] JoystickController _moveJoystick;
    [SerializeField] Transform _playerCamera;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        ApplyGravity();
        ThrowRaycast();
    }

    void Move()
    {
        Vector3 cameraForward = _playerCamera.transform.forward;
        Vector3 cameraRight = _playerCamera.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction = cameraForward * _moveJoystick.Movement.y + cameraRight * _moveJoystick.Movement.x;
        direction = Vector3.ClampMagnitude(direction, 1f);

        float targetSpeed = direction.magnitude * _moveSpeed;
        _currentSpeed = Mathf.MoveTowards(_currentSpeed, targetSpeed, _moveSpeed / _accelerationTime * Time.deltaTime);

        _currentMove = direction * _currentSpeed;

        if (_moveJoystick.Movement.magnitude < 0.01f)
        {
            _currentSpeed = 0;
            _currentMove = Vector3.zero;
        }

        if (_currentMove.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_currentMove.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        TryStep(direction);

        _time += Time.deltaTime;
        if (_moveJoystick.Movement == Vector2.zero)
            _time = 0f;

        float speed = Mathf.Lerp(0, 1, _time / 0.5f);

        _controller.Move((_currentMove + _velocity) * Time.deltaTime * speed);
    }

    void TryStep(Vector3 direction)
    {
        if (direction.magnitude < 0.1f) return;
        if (!_controller.isGrounded) return;

        Vector3 originLow = transform.position + Vector3.up * 0.05f;
        Vector3 originHigh = transform.position + Vector3.up * (_stepHeight + 0.1f);

        if (Physics.Raycast(originLow, direction, out RaycastHit hitLow, _stepCheckDistance))
        {
            if (!Physics.Raycast(originHigh, direction, _stepCheckDistance))
            {
                _controller.Move(Vector3.up * _stepHeight);
            }
        }
    }

    void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }

    public void Jump()
    {
        if (!_controller.isGrounded) return;

        _velocity.y = Mathf.Sqrt(_jumpForce * -1f * _gravity);
    }

    void ThrowRaycast()
    {
        Vector3 direction = _playerCamera.forward;
        Vector3 start = transform.position + Vector3.forward * 0.5f;

        Ray ray = new(start, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
        {
            Debug.Log(hit.collider.name);
        }

        Debug.DrawRay(start, direction * 0.5f, Color.blue, 999f);
    }
}