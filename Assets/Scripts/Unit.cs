using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum ActivityState
    {
        Idle,
        Moving,
        MeleeAttacking,
        RangeAttacking,
        RangeAiming,
    }

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
    private WalkableArea _walkableAreaCache;
    [ShowInInspector] private ActivityState _state;

    public bool IsPlayer { get => _isPlayer; }
    public int MovementPoints { get => _movementPoints; }
    public int CurrentMovementPoints { get => _currentMovementPoints; }
    public Actor Actor { get => _actor; }
    public int NodeIndex { get => _actor.NodeIndex; }
    public Animator Animator { get => _animator; }
    public List<Ability> Abilities { get => _abilities; }
    public Ability MoveAbility { get => _moveAbility; }
    public Vector3 WorldPosition { get => transform.position; }
    public SelectionIndicator SelectionIndicator { get; private set ; }

    private void Awake()
    {
        _currentMovementPoints = _movementPoints;
    }

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _actor = GetComponent<Actor>();
        SelectionIndicator = GetComponent<SelectionIndicator>();
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

    public async Task MoveAlongPath(Path path, CancellationToken cancellationToken)
    {
        _currentMovementPoints -= path.cost;
        NullifyWalkableArea();
        Animator.SetBool("Running", true);
        await _actor.MoveAlongPathAsync(path, cancellationToken);
        Animator.SetBool("Running", false);
    }

    public Task FaceTowards(Vector3 worldPosition)
    {
        return Actor.FaceTowardsAsync(worldPosition);
    }

    internal void RefreshOnNewTurn()
    {
        _currentMovementPoints = _movementPoints;
    }

    public void SetState(ActivityState newState)
    {
        _state = newState;

        switch (_state)
        {
            case ActivityState.Idle:
                // _animator.SetBool();
                break;
            case ActivityState.Moving:
                break;
            case ActivityState.MeleeAttacking:
                break;
            case ActivityState.RangeAttacking:
                break;
            case ActivityState.RangeAiming:
                break;
            default:
                break;


        }

    }
}
