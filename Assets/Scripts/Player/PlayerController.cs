using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject interactButton;

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

    string _interactTag = "Interactable";
    string _waterTag = "Water";

    bool _intoWater;

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

        _time += Time.deltaTime;
        if (_moveJoystick.Movement == Vector2.zero)
            _time = 0f;

        float speed = Mathf.Lerp(0, 1, _time / 0.5f);

        Debug.Log($"{(_currentMove + _velocity) * Time.deltaTime * speed} -- {_velocity} -- {speed}");
        _controller.Move((_currentMove) * Time.deltaTime * speed + _velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (IsGrounded() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    public void Jump()
    {
        if (!IsGrounded() || _intoWater) return;

        _velocity.y = Mathf.Sqrt(_jumpForce * -1f * _gravity);
    }

    void ThrowRaycast()
    {
        Vector3 direction = _playerCamera.forward;
        Vector3 start = transform.position;

        Ray ray = new(start, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, 1f))
        {
            if (hit.collider.CompareTag(_interactTag))
            {
                interactButton.SetActive(true);
                return;
            }
        }

        interactButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        _intoWater = other.CompareTag(_waterTag);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_waterTag))
            _intoWater = false;
    }
}