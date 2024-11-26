using System;
using Navigation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    WalkableAreaVisualizer _areaVisualizer;
    NavGrid _grid;
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }


    public NavGrid Grid { get => _grid; }
    public WalkableAreaVisualizer AreaVisualizer { get => _areaVisualizer; }

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
    }

    private void InitilizeWalkableAreaVisualizer()
    {
        if (_areaVisualizer == null)
        {
            //TO DO - load prefab via addressables
            Debug.Log("TO DO - load prefab via addressables");
            Debug.Log("<color=red> NO WALKABLE AREA VISUALIZER </color>");
        }
    }
}
