using Navigation;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Animator _animator;
    private Actor _actor;
    [SerializeField]
    private bool _isPlayer = false;
    [SerializeField]
    private int _movementPoints = 10;
    private int _currentMovementPoints;
    WalkableArea _walkableArea;

    public bool IsPlayer { get => _isPlayer; }
    public int MovementPoints { get => _movementPoints; }
    public int CurrentMovementPoints { get => _currentMovementPoints; }
    public Actor Actor { get => _actor; }
    public int NodeIndex { get => _actor.NodeIndex; }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _actor = GetComponent<Actor>();
        _currentMovementPoints = _movementPoints;
    }

    public WalkableArea GetWalkableArea()
    {
        if (_walkableArea == null)
        {
            _walkableArea = Pathfinder.FindWalkableArea(LevelManager.Instance.Grid, NodeIndex, _currentMovementPoints);
        }

        return _walkableArea;
    }

    public void NullifyWalkableArea()
    {
        _walkableArea = null;
    }

}
