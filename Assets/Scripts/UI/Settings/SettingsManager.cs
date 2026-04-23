using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// менеджер настроек
/// </summary>
public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] Toggle toggle;
    [SerializeField] RectTransform handle;
    [SerializeField] Image background;
    [SerializeField] Sprite spriteOn;
    [SerializeField] Sprite spriteOff;

    public bool SoundActive { get; private set; }

    Coroutine _toggleCoroutine;

    /// <summary>
    /// старт
    /// </summary>
    void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    /// <summary>
    /// изменение значение тогла
    /// </summary>
    /// <param name="newValue"> новое значение </param>
    public void OnToggleValueChanged(bool newValue)
    {
        if (_toggleCoroutine != null)
            StopCoroutine(_toggleCoroutine);

        _toggleCoroutine = StartCoroutine(ToggleAnimation(newValue ? 65 : -65, newValue ? spriteOn : spriteOff));

        SoundActive = newValue;
    }

    /// <summary>
    /// анимация переключателя
    /// </summary>
    /// <param name="targetX"> целевой x </param>
    /// <param name="backgroundSprite"> спрайт фона </param>
    /// <returns> интерфейс </returns>
    IEnumerator ToggleAnimation(float targetX, Sprite backgroundSprite)
    {
        background.sprite = backgroundSprite;

        float startX = handle.anchoredPosition.x;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float currentX = Mathf.Lerp(startX, targetX, elapsed / duration);
            handle.anchoredPosition = new(currentX, handle.anchoredPosition.y);

            yield return null;
        }
    }

    /// <summary>
    /// открытие окна настроек
    /// </summary>
    public void OpenSettings()
    {
        settingsCanvas.SetActive(true);

        bool soundActive = PlayerPrefs.GetInt(GameManager.SoundActive) == 1;
        toggle.isOn = soundActive;
        OnToggleValueChanged(soundActive);
    }

    /// <summary>
    /// закрытие окна настроек
    /// </summary>
    public void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }

    /// <summary>
    /// Сохранение настроек
    /// </summary>
    public void Save()
    {
        PlayerPrefs.SetInt(GameManager.SoundActive, SoundActive ? 1 : 0);
        //PlayerPrefs.Save();

        CloseSettings();
    }
}
