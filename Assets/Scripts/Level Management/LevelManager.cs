using System;
using System.Collections.Generic;
using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    WalkableAreaVisualizer _areaVisualizer;

    [SerializeField, Required]
    PathVisualizer _pathVisualizer;

    [SerializeField, Required]
    CameraController _camController;
    NavGrid _grid;
    private static LevelManager _instance;
    private List<Unit> _playerUnits;
    private List<Unit> _enemyUnits;


    public static LevelManager Instance { get { return _instance; } }
    public CameraController CamController { get => _camController; }

    public NavGrid Grid { get => _grid; }
    public List<Unit> PlayerUnits { get => _playerUnits; }
    public List<Unit> EnemyUnits { get => _enemyUnits; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            InitializeOnAwake();
        }
    }

    private void InitializeOnAwake()
    {
        _grid = FindAnyObjectByType<NavGrid>();
        InitilizeWalkableAreaVisualizer();
        InitilizeUnitList();
    }

    private void InitilizeWalkableAreaVisualizer()
    {
        if (_areaVisualizer == null)
        {
            //TO DO - load prefab via addressables
            Debug.Log("TO DO - load prefab via addressables");
            Debug.Log("<color=red> NO WALKABLE AREA VISUALIZER </color>");
        }

        _areaVisualizer.Initialize(_grid.TileSize);
    }

    private void InitilizeUnitList()
    {
        _playerUnits = new();
        _enemyUnits = new();

        Unit unit;

        List<Actor> actors = _grid.GetActors();
        foreach (var actor in actors)
        {
            unit = actor.gameObject.GetComponent<Unit>();
            if (unit != null)
            {
                if (unit.IsPlayer)
                {
                    _playerUnits.Add(unit);
                }
                else
                {
                    _enemyUnits.Add(unit);
                }
            }

        }
    }

    public void ShowPathPreview(Path path)
    {
        _pathVisualizer.UpdatePath(path);
        _pathVisualizer.Show();
    }

    public void HidePathPreview()
    {
        _pathVisualizer.Hide();
    }

    public void ShowWalkableArea(WalkableArea area)
    {
        _areaVisualizer.UpdateWalkableArea(area);
        _areaVisualizer.Show();
    }

    public void HideWalkableArea()
    {
        _areaVisualizer.Hide();
    }

    public void NullifyPlayerWalkableAreas()
    {
        foreach (var unit in _playerUnits)
        {
            unit.NullifyWalkableArea();
        }
    }
}
