using System;
using Navigation;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    NavGrid _grid;
    private static LevelManager _instance;
    public static LevelManager Instance { get { return _instance; } }

    public NavGrid Grid { get => _grid; }

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
        _grid = FindAnyObjectByType<NavGrid>();
    }
}
