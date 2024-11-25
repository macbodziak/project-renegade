using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectSelectionIndicator : SelectionIndicator
{
    [SerializeField]
    [Required]
    GameObject _prefab;

    GameObject _highlightObject;

    [Title("OScillation settings")]
    [SerializeField]
    [MinValue(0)]
    [MaxValue(1)]
    [SuffixLabel("* scale", Overlay = true)]
    float _amplitude = 0.063f;

    [SerializeField]
    [MinValue(0)]
    [SuffixLabel("1/s", Overlay = true)]
    float _frequency = 1.75f;

    public void Awake()
    {
        _highlightObject = Instantiate<GameObject>(_prefab, transform);
        _highlightObject.SetActive(false);
    }



    protected override void OnEnterState()
    {

        switch (State)
        {
            case SelectionIndicatorState.InActive:
                _highlightObject.SetActive(false);
                break;
            case SelectionIndicatorState.Selected:
                _highlightObject.SetActive(true);
                break;
            case SelectionIndicatorState.Reviewed:
                _highlightObject.SetActive(true);
                StartCoroutine(Oscillate());
                break;
        }
    }

    private IEnumerator Oscillate()
    {
        float angularVelocity = _frequency * 2f * Mathf.PI;
        float baseScale_x = _highlightObject.transform.localScale.x;
        float baseScale_z = _highlightObject.transform.localScale.z;

        while (State == SelectionIndicatorState.Reviewed)
        {
            float newScale_x = baseScale_x * (1 + Mathf.Sin(Time.time * angularVelocity) * _amplitude);
            float newScale_z = baseScale_z * (1 + Mathf.Sin(Time.time * angularVelocity) * _amplitude);
            _highlightObject.transform.localScale = new Vector3(newScale_x, transform.localScale.y, newScale_z);
            yield return null;
        }
        _highlightObject.transform.localScale = new Vector3(baseScale_x, _highlightObject.transform.localScale.y, baseScale_z);
    }
}
