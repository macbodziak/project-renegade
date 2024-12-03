using System;
using System.Collections.Generic;
using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Animator _animator;
    private Actor _actor;
    [SerializeField]
    private bool _isPlayer = false;
    [SerializeField]
    private int _movementPoints = 10;
    [SerializeField]
    [Sirenix.OdinInspector.ReadOnly]
    private int _currentMovementPoints;
    [SerializeField] private Ability _moveAbility;
    [SerializeField] private List<Ability> _abilities;
    WalkableArea _walkableAreaCache;

    public bool IsPlayer { get => _isPlayer; }
    public int MovementPoints { get => _movementPoints; }
    public int CurrentMovementPoints { get => _currentMovementPoints; }
    public Actor actor { get => _actor; }
    public int NodeIndex { get => _actor.NodeIndex; }
    public Animator animator { get => _animator; }
    public List<Ability> Abilities { get => _abilities; }
    public Ability MoveAbility { get => _moveAbility; }

    private void Awake()
    {
        _currentMovementPoints = _movementPoints;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _actor = GetComponent<Actor>();
    }

    public WalkableArea GetWalkableArea()
    {
        if (_walkableAreaCache == null)
        {
            _walkableAreaCache = Pathfinder.FindWalkableArea(LevelManager.Instance.Grid, NodeIndex, _currentMovementPoints);
        }

        return _walkableAreaCache;
    }

    public void NullifyWalkableArea()
    {
        _walkableAreaCache = null;
    }

    public void MoveAlongPath(Path path)
    {
        _currentMovementPoints -= path.cost;
        _actor.MoveAlongPath(path);
        NullifyWalkableArea();
    }

    internal void RefreshOnNewTurn()
    {
        _currentMovementPoints = _movementPoints;
    }
}
