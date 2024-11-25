using UnityEngine;

public class MaterialHighlighter : Highlighter, IHighlighter
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
                _renderer.material = _originalMaterial;
                break;
            case HighlightState.Selected:
                _renderer.material = _selectedMaterial;
                break;
            case HighlightState.Checked:
                _renderer.material = _checkedMaterial;
                break;
        }
    }
}
