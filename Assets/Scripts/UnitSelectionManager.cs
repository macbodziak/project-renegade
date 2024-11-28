using System;
using Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    [SerializeField]
    Unit _selectedUnit;

    private static UnitSelectionManager _instance;
    public static UnitSelectionManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            InitializationStateOnAwake();
        }
    }

    private void InitializationStateOnAwake()
    {
        _selectedUnit = null;
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (unit == _selectedUnit)
        {
            return;
        }

        SelectionIndicator indicator;

        if (_selectedUnit != null)
        {
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
        }

        UpdateSelectedUnit(unit);

        Debug.Log("SetSelectedUnit -- " + unit.name);
        //TODO - add event trigger
    }


    public void CancelSelection()
    {
        if (_selectedUnit != null)
        {
            SelectionIndicator indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
            LevelManager.Instance.AreaVisualizer.Hide();
            InputManager.Instance.SetState(InputManager.State.SelectUnit);
        }
        _selectedUnit = null;
    }

    private void UpdateSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SelectionIndicator indicator = _selectedUnit.GetComponent<SelectionIndicator>();
        if (indicator != null)
        {
            indicator.IsActive = true;
        }

        InputManager.Instance.SetState(InputManager.State.SelectMovementTarget);
        WalkableArea wa = _selectedUnit.GetWalkableArea();
        LevelManager.Instance.AreaVisualizer.UpdateWalkableArea(wa);
        LevelManager.Instance.AreaVisualizer.Show();
    }

}
