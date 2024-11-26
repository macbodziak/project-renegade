using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Navigation;

public class MosaicVisualizer : WalkableAreaVisualizer
{
    [SerializeField]
    [PreviewField, Title("Tile Prefab")]
    public GameObject _tilePrefab;

    [SerializeField]
    List<GameObject> _tiles;
    bool _dirty;

    void Awake()
    {
        int initialCapacity = 20;
        _tiles = new List<GameObject>(initialCapacity);
        for (int i = 0; i < initialCapacity; i++)
        {
            _tiles.Add(Instantiate<GameObject>(_tilePrefab, transform));
            _tiles[i].SetActive(false);
        }

        _dirty = true;
    }

    public override void Hide()
    {
        _isShowing = false;
        gameObject.SetActive(false);
    }

    public override void Show()
    {
        if (_area == null)
        {
            return;
        }

        _isShowing = true;
        gameObject.SetActive(true);
        if (_dirty)
        {
            RefreshArea();
        }
        DisplayArea();
    }

    public override void UpdateWalkableArea(WalkableArea area)
    {
        if (_area == area)
        {
            return;
        }

        _area = area;
        _dirty = true;
    }

    private void DisplayArea()
    {
        WalkableAreaElement[] elements = _area.GetWalkableAreaElements();
        for (int i = 0; i < _area.Count(); i++)
        {
            _tiles[i].transform.position = elements[i].worldPosition;
            _tiles[i].SetActive(true);
        }
    }

    private void RefreshArea()
    {
        if (_area.Count() > _tiles.Count)
        {
            for (int i = _tiles.Count; i < _area.Count(); i++)
            {
                _tiles.Add(Instantiate<GameObject>(_tilePrefab, transform));
            }
        }
        else
        {
            for (int i = _area.Count() - 1; i < _tiles.Count; i++)
            {
                _tiles[i].SetActive(false);
            }
        }

        _dirty = false;
    }
}
