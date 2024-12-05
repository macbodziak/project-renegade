using Sirenix.OdinInspector;
using UnityEngine;

public class UIBlinker : MonoBehaviour
{
    [SerializeField] float _frequency;
    [SerializeField] float _maxAlpha;
    [SerializeField] float _minAlpha;
    CanvasRenderer _canvasRenderer;
    bool _isEnabled;
    bool _stopRequested;

    void Start()
    {
        _canvasRenderer = GetComponent<CanvasRenderer>();
    }

    [Button("start")]
    public void StartBlinking()
    {
        if (_isEnabled) return;

        _stopRequested = false;
        _isEnabled = true;
        Blink();
    }

    [Button("stop")]
    public void StopBlinking()
    {
        _stopRequested = true;
    }

    private async void Blink()
    {
        _isEnabled = true;
        float angularVelocity = _frequency * 2f * Mathf.PI;
        float amplitude = Mathf.Clamp(_maxAlpha - _minAlpha, 0f, 1f);
        float offset = 1f - amplitude;
        while (_stopRequested == false)
        {
            float alpha = offset + amplitude * Mathf.Sin(Time.time * angularVelocity) * 0.5f;
            _canvasRenderer.SetAlpha(alpha);
            await Awaitable.NextFrameAsync();
        }
        _isEnabled = false;
    }
}
