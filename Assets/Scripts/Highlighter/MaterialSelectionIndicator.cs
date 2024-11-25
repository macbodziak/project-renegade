using UnityEngine;

public class MaterialSelectionIndicator : SelectionIndicator, ISelectionIndicator
{
    Material _originalMaterial;
    [SerializeField]
    Material _selectedMaterial;
    [SerializeField]
    Material _checkedMaterial;
    Renderer _renderer;


    public void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _originalMaterial = _renderer.material;
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
                _renderer.material = _originalMaterial;
                break;
            case SelectionIndicatorState.Selected:
                _renderer.material = _selectedMaterial;
                break;
            case SelectionIndicatorState.Checked:
                _renderer.material = _checkedMaterial;
                break;
        }
    }
}
