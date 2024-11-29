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
    WalkableArea _walkableAreaCache;

    public bool IsPlayer { get => _isPlayer; }
    public int MovementPoints { get => _movementPoints; }
    public int CurrentMovementPoints { get => _currentMovementPoints; }
    public Actor actor { get => _actor; }
    public int NodeIndex { get => _actor.NodeIndex; }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _actor = GetComponent<Actor>();
        _currentMovementPoints = _movementPoints;
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
        _actor.MoveAlongPath(path);
        NullifyWalkableArea();
    }

}
