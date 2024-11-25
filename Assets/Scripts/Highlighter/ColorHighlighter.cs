using UnityEngine;

public class ColorHighlighter : Highlighter, IHighlighter
{
    [SerializeField]
    Color _selectedTint;
    [SerializeField]
    Color _checkedTint;
    Color _originTint;
    Material _material;

    public void Start()
    {
        _material = GetComponentInChildren<MeshRenderer>().material;
        Debug.Assert(_material != null);
        _originTint = _material.HasProperty("_BaseColor") ? _material.GetColor("_BaseColor") : _material.color;
    }

    public override HighlightState State
    {
        get
        {
            return _state;
        }
        set
        {
            SetState(value);
        }
    }

    private void SetState(HighlightState value)
    {
        _state = value;

        switch (_state)
        {
            case HighlightState.InActive:
                _material.SetColor("_BaseColor", _originTint);
                break;
            case HighlightState.Selected:
                _material.SetColor("_BaseColor", _selectedTint);
                break;
            case HighlightState.Checked:
                _material.SetColor("_BaseColor", _checkedTint);
                break;
        }
    }
}
