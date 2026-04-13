using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// менеджер загрузки
/// </summary>
public class LoadManager : MonoBehaviour
{
    [SerializeField] GameObject loadCanvas;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI textProgress;
    [SerializeField] float duration;

    float _currentTime = 0f;
    string _text = "Загрузка.. ({0:P0})";
    bool _isLoading = false;

    

    /// <summary>
    /// андейт
    /// </summary>
    void Update()
    {
        if (_isLoading) 
            Load();
    }

    /// <summary>
    /// старт загрузки
    /// </summary>
    public void StartLoad()
    {
        loadCanvas.SetActive(true);
        _isLoading = true;
    }

    /// <summary>
    /// логика загрузки
    /// </summary>
    void Load()
    {
        if (_currentTime < duration)
        {
            _currentTime += Time.deltaTime;

            float progress = _currentTime / duration;
            slider.value = progress;
            textProgress.text = string.Format(_text, progress);

            return;
        }

        _isLoading = false;
        loadCanvas.SetActive(false);
    }
}
