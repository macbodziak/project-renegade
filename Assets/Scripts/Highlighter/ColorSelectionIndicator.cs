using UnityEngine;

public class ColorSelectionIndicator : SelectionIndicator, ISelectionIndicator
{
    [SerializeField]
    Color _selectedTint;
    [SerializeField]
    Color _checkedTint;
    Color _originTint;
    Material _material;

    public void Start()
    {
        _material = GetComponentInChildren<Renderer>().material;
        Debug.Assert(_material != null);
        _originTint = _material.HasProperty("_BaseColor") ? _material.GetColor("_BaseColor") : _material.color;
    }

    public override SelectionIndicatorState State
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

    private void SetState(SelectionIndicatorState value)
    {
        _state = value;

        switch (_state)
        {
            case SelectionIndicatorState.InActive:
                _material.SetColor("_BaseColor", _originTint);
                break;
            case SelectionIndicatorState.Selected:
                _material.SetColor("_BaseColor", _selectedTint);
                break;
            case SelectionIndicatorState.Checked:
                _material.SetColor("_BaseColor", _checkedTint);
                break;
        }
    }
}
