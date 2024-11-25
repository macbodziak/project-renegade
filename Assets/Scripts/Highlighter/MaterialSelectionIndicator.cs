using UnityEngine;

public class MaterialSelectionIndicator : SelectionIndicator
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


    protected override void OnEnterState()
    {
        switch (State)
        {
            case SelectionIndicatorState.InActive:
                _renderer.material = _originalMaterial;
                break;
            case SelectionIndicatorState.Selected:
                _renderer.material = _selectedMaterial;
                break;
            case SelectionIndicatorState.Reviewed:
                _renderer.material = _checkedMaterial;
                break;
        }
    }
}
