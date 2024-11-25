using System;
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
        SelectionIndicator indicator;

        if (unit != _selectedUnit && _selectedUnit != null)
        {
            indicator = _selectedUnit.GetComponent<SelectionIndicator>();
            if (indicator != null)
            {
                indicator.IsActive = false;
            }
        }

        _selectedUnit = unit;
        indicator = _selectedUnit.GetComponent<SelectionIndicator>();
        if (indicator != null)
        {
            indicator.IsActive = true;
        }

        Debug.Log("SetSelectedUnit -- " + unit.name);
        //TODO - add event trigger
    }

    public void UnselectUnit()
    {
        _selectedUnit = null;
    }



}
