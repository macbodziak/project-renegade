using UnityEngine;

public class ColorSelectionIndicator : SelectionIndicator
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


    protected override void OnEnterState()
    {
        switch (State)
        {
            case SelectionIndicatorState.InActive:
                _material.SetColor("_BaseColor", _originTint);
                break;
            case SelectionIndicatorState.Selected:
                _material.SetColor("_BaseColor", _selectedTint);
                break;
            case SelectionIndicatorState.Reviewed:
                _material.SetColor("_BaseColor", _checkedTint);
                break;
        }
    }
}
