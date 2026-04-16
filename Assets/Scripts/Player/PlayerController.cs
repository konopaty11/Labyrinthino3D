using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// класс игрока
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject interactButton;
    [SerializeField] Vector3 positionToCarry;
    [SerializeField] LayerMask mask;
    [SerializeField] PasswordManager passwordManager;

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
    string _zoneToPutTag = "ZoneToPut";

    bool _intoWater;
    GameObject _carryObject;
    IInteractable _currentInteractable;

    [Header("Input")]
    [SerializeField] JoystickController _moveJoystick;
    [SerializeField] Transform _playerCamera;

    public static event UnityAction PutItem;
    public static event UnityAction<ItemType> PickupItem;

    /// <summary>
    /// старт
    /// </summary>
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    /// <summary>
    /// апдейт
    /// </summary>
    void Update()
    {
        Move();
        ApplyGravity();
        ThrowRaycast();
    }

    /// <summary>
    /// движение
    /// </summary>
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

        _controller.Move((_currentMove) * Time.deltaTime * speed + _velocity * Time.deltaTime);
    }

    /// <summary>
    /// логика гравитации
    /// </summary>
    void ApplyGravity()
    {
        if (IsGrounded() && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
    }

    /// <summary>
    /// на земле ли игрок
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    /// <summary>
    /// прыжок
    /// </summary>
    public void Jump()
    {
        if (!IsGrounded() || _intoWater) return;

        _velocity.y = Mathf.Sqrt(_jumpForce * -1f * _gravity);
    }

    /// <summary>
    /// бросает рейксаст
    /// </summary>
    void ThrowRaycast()
    {
        Vector3 direction = _playerCamera.forward;
        Vector3 start = _playerCamera.position;

        Ray ray = new(start, direction);

        if (Physics.Raycast(start, direction, out RaycastHit hit, 15f, mask))
        {
            Vector3 pointOnTarget = hit.collider.ClosestPoint(_controller.bounds.center);
            Vector3 pointOnPlayer = _controller.ClosestPoint(pointOnTarget);

            float distance = Vector3.Distance(pointOnPlayer, pointOnTarget);

            if (hit.collider.CompareTag(_interactTag) && distance <= 0.5f)
            {
                _currentInteractable = hit.collider.GetComponent<IInteractable>();
                interactButton.SetActive(true);
                return;
            }
        }

        Debug.DrawRay(start, direction * 4, Color.magenta, 0.1f);

        interactButton.SetActive(false);
    }

    /// <summary>
    /// взаимодействие
    /// </summary>
    public void Interact()
    {
        if (_currentInteractable is ItemToCarry itemToCarry)
        {
            if (_carryObject != null)
                return;

            PickupItem?.Invoke(itemToCarry.itemType);
            _carryObject = itemToCarry.gameObject;
            _carryObject.transform.SetParent(_playerCamera);
            _carryObject.transform.localPosition = positionToCarry;
        }
        else if (_currentInteractable is Door door)
        {
            _currentInteractable.Interact();
        }
        else if (_currentInteractable is PasswordPart passwordPart)
        {
            passwordManager.SetNextNumber(passwordPart.symbol);
        }
    }

    /// <summary>
    /// вход в коллайдер триггера
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_waterTag))
        {
            _intoWater = true;
        }
        else if (other.CompareTag(_zoneToPutTag))
        {
            PutItem?.Invoke();
            Destroy(_carryObject);
        }
    }

    /// <summary>
    /// выход из коллайдера триггера
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_waterTag))
            _intoWater = false;
    }
}