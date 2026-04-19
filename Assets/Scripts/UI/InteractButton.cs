using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image image;
    [SerializeField] Sprite activeSprite;
    [SerializeField] Sprite inactiveSprite;

    public void SetActive(bool active)
    {
        image.sprite = active ? activeSprite : inactiveSprite;
        canvasGroup.interactable = active;
    }
}
