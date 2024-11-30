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

    public override void Initialize(float tileSize)
    {
        _tileSize = tileSize;
        int initialCapacity = 20;
        _tiles = new List<GameObject>(initialCapacity);
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject newTile = Instantiate<GameObject>(_tilePrefab, transform);
            newTile.transform.localScale *= tileSize;
            _tiles.Add(newTile);
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
        if (_dirty)
        {
            RefreshArea();
        }

        _isShowing = true;
        gameObject.SetActive(true);

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
        if (_area == null)
        {
            return;
        }

        WalkableAreaElement[] elements = _area.GetWalkableAreaElements();
        for (int i = 0; i < _area.Count(); i++)
        {
            _tiles[i].transform.position = elements[i].worldPosition;
            _tiles[i].SetActive(true);
        }
    }

    private void RefreshArea()
    {
        if (_area == null)
        {
            foreach (var tile in _tiles)
            {
                tile.SetActive(false);
            }
            return;
        }

        if (_area.Count() > _tiles.Count)
        {
            for (int i = _tiles.Count; i < _area.Count(); i++)
            {
                GameObject newTile = Instantiate<GameObject>(_tilePrefab, transform);
                newTile.transform.localScale *= _tileSize;
                _tiles.Add(newTile);
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
