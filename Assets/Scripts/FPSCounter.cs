using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        _text.text = "FPS: " + Mathf.RoundToInt(fps);
    }
}