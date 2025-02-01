using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Actor _actor;
    [SerializeField] private bool _isPlayer = false;
    [SerializeField] private int _movementPoints = 10;

    [SerializeField] [Sirenix.OdinInspector.ReadOnly]
    private int _currentMovementPoints;
    [SerializeField] private Ability _moveAbility;
    [SerializeField] private List<Ability> _abilities;
    [SerializeField] private UnitAnimationHandler _animationHandlerConfig;
    private UnitAnimationHandler _animationHandler;
    private WalkableArea _walkableAreaCache;

    public bool IsPlayer { get => _isPlayer; }

    public int MovementPoints { get => _movementPoints; }

    public int CurrentMovementPoints { get => _currentMovementPoints; }

    public Actor Actor { get => _actor; }

    public int NodeIndex { get => _actor.NodeIndex; }

    public List<Ability> Abilities { get => _abilities; }

    public Ability MoveAbility { get => _moveAbility; }

    public Vector3 WorldPosition { get => transform.position; }

    public SelectionIndicator SelectionIndicator { get; private set; }

    public UnitAnimationHandler AnimationHandler { get => _animationHandler; }

    private void Awake()
    {
        _currentMovementPoints = _movementPoints;
    }

    private void Start()
    {
        _animationHandler = GetComponentInChildren<UnitAnimationHandler>();
        if (!_animationHandler)
        {
            throw new MissingComponentException("Unit animation handler not found");
        }
        
        _actor = GetComponent<Actor>();
        SelectionIndicator = GetComponent<SelectionIndicator>();
    }

    public WalkableArea GetWalkableArea()
    {
        if (_walkableAreaCache == null)
        {
            _walkableAreaCache = Pathfinder.FindWalkableArea(LevelManager.Instance.Grid,
                NodeIndex, _currentMovementPoints);
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
        _animationHandler.PlayRunningAnimation();
        await _actor.MoveAlongPathAsync(path, cancellationToken);
        _animationHandler.StopRunningAnimation();
    }

    public Task FaceTowards(Vector3 worldPosition)
    {
        return Actor.FaceTowardsAsync(worldPosition);
    }

    internal void RefreshOnNewTurn()
    {
        _currentMovementPoints = _movementPoints;
    }
}