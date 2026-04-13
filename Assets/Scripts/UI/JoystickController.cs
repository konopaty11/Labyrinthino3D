using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] float _maxDistance;

    public Vector2 Movement { get; private set; }
    public Vector2 Direction { get; private set; }

    RectTransform _joystick;
    Vector2 _startPosition;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _joystick = GetComponent<RectTransform>();
        _startPosition = _joystick.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 target = _joystick.anchoredPosition + eventData.delta;

        float distance = Vector2.Distance(_startPosition, target);
        float clampedDistance = Mathf.Clamp(distance, 0f, _maxDistance);
        Vector2 direction = (target - _startPosition).normalized;

        Movement = clampedDistance * direction;
        Direction = direction;
        _joystick.anchoredPosition = _startPosition + clampedDistance * direction;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _joystick.anchoredPosition = _startPosition;
        Movement = Vector2.zero;
        Direction = Vector2.zero;
    }
}
