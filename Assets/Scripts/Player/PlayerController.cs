using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// класс игрока
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] InteractButton interactButton;
    [SerializeField] Vector3 positionToCarry;
    [SerializeField] LayerMask mask;
    [SerializeField] PasswordManager passwordManager;
    [SerializeField] Material ballMaterial;
    [SerializeField] GameObject ballPrefab;

    [SerializeField] float _moveSpeed = 1.7f;
    [SerializeField] float _accelerationTime = 0.5f;
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] float _jumpForce = 5f;

    [Header("Step settings")]
    [SerializeField] float _stepHeight = 0.4f;
    [SerializeField] float _stepCheckDistance = 0.5f;

    Queue<GameObject> _balls = new();

    public Vector3 PlatformOffset
    {
        get
        {
            if (_platform == null)
                return Vector3.zero;

            return _platform.PlatformOffset;
        }
    }

    public Vector3 PlatformVelocity
    {
        get 
        { 
            if (_platform == null)
                return Vector3.zero;

            return _platform.Velocity; 
        } 
    }

    public BallColorType CurrentColorType { get; private set; }

    public GameObject ThrowingBall { get; set; }

    CharacterController _controller;
    Vector3 _externalVelocity;
    Vector3 _velocity;
    Vector3 _currentMove;

    float _currentSpeed;
    float _time;

    string _interactTag = "Interactable";
    string _waterTag = "Water";
    string _zoneToPutTag = "ZoneToPut";
    string _movingPlatformTag = "MovingPlatform";

    bool _intoWater;
    bool _isGrounded;
    ItemToCarry _itemCarry;
    IInteractable _currentInteractable;
    MovingPlatform _platform;

    [Header("Input")]
    [SerializeField] JoystickController _moveJoystick;
    [SerializeField] Transform _playerCamera;

    public static event UnityAction<ItemType> PutItem;
    public static event UnityAction<ItemType> PickupItem;

    /// <summary>
    /// старт
    /// </summary>
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        ballMaterial.color = Color.yellow;
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

        _controller.Move((_currentMove) * Time.deltaTime * speed + (_velocity + _externalVelocity) * Time.deltaTime + PlatformOffset);
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

        _externalVelocity = Vector3.Lerp(_externalVelocity, Vector3.zero, Time.deltaTime);
    }

    /// <summary>
    /// на земле ли игрок
    /// </summary>
    /// <returns></returns>
    bool IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.1f);

        if (_isGrounded && hit.collider.CompareTag(_movingPlatformTag))
            _platform = hit.collider.GetComponent<MovingPlatform>();
        else
            _platform = null;

        return _isGrounded;
    }

    /// <summary>
    /// прыжок
    /// </summary>
    public void Jump()
    {
        if (!_isGrounded || _intoWater) return;

        _externalVelocity = PlatformVelocity;
        _velocity.y = Mathf.Sqrt(_jumpForce * -1f * _gravity);
    }

    /// <summary>
    /// бросает рейксаст
    /// </summary>
    void ThrowRaycast()
    {
        Vector3 direction = _playerCamera.forward;
        Vector3 start = _playerCamera.position;

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

        //Debug.DrawRay(start, direction * 4, Color.magenta, 0.1f);
        interactButton.SetActive(ThrowingBall != null);
    }

    /// <summary>
    /// взаимодействие
    /// </summary>
    public void Interact()
    {
        if (_currentInteractable is ItemToCarry itemToCarry)
        {
            if (_itemCarry != null)
                return;

            PickupItem?.Invoke(itemToCarry.itemType);
            _itemCarry = itemToCarry;

            _itemCarry.transform.SetParent(_playerCamera);
            _itemCarry.transform.localPosition = positionToCarry;

            if (_itemCarry.itemType == ItemType.Ball)
                CurrentColorType = BallColorType.Yellow;
        }
        else if (_currentInteractable is Door door)
        {
            _currentInteractable.Interact();
        }
        else if (_currentInteractable is PasswordPart passwordPart)
        {
            PickupItem?.Invoke(ItemType.Password);
            passwordManager.SetNextNumber(passwordPart.symbol);

            Destroy(passwordPart.gameObject);
        }
        else if (_currentInteractable is BallColorInteractable ballColor)
        {
            if (_itemCarry == null || _itemCarry.itemType != ItemType.Ball)
                return;

            CurrentColorType = ballColor.color;
            ballMaterial.color = GetColor(ballColor.color);
            Destroy(ballColor.gameObject);
        }
        else if (ThrowingBall != null)
        {
            Destroy(ThrowingBall);

            GameObject ballObject = Instantiate(ballPrefab, _playerCamera.position, Quaternion.identity);

            _balls.Enqueue(ballObject);
            if (_balls.Count > 10) 
                Destroy(_balls.Dequeue());

            Rigidbody rg = ballObject.GetComponent<Rigidbody>();
            rg.velocity = _playerCamera.forward * 3f;

            ThrowingBall ball = ballObject.GetComponent<ThrowingBall>();
            ball.Throw();
        }
    }

    Color GetColor(BallColorType type)
    {
        return type switch
        {
            BallColorType.Blue => Color.blue,
            BallColorType.Yellow => Color.yellow,
            BallColorType.Red => Color.red,
        };
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
        else if (other.CompareTag(_zoneToPutTag) && _itemCarry != null)
        {
            PutItem?.Invoke(_itemCarry.itemType);
            Destroy(_itemCarry.gameObject);
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