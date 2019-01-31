using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPS : MonoBehaviour
{
    const float SmoothFactor = 0.1f;

    TextMeshProUGUI _text;
    float _smoothDeltaTime;

    void Awake() =>
        _text = GetComponent<TextMeshProUGUI>();

    void Start() =>
        _smoothDeltaTime = Time.unscaledDeltaTime;

    void Update()
    {
        _smoothDeltaTime += (Time.unscaledDeltaTime - _smoothDeltaTime) *
            SmoothFactor;

        _text.text = Mathf.RoundToInt(1f / _smoothDeltaTime).ToString();
    }
}