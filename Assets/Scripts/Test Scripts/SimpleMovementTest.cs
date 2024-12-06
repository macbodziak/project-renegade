using Navigation;
using UnityEngine;

public class SimpleMovementTest : MonoBehaviour
{
    [SerializeField]
    Actor actor;
    [SerializeField]
    NavGrid grid;

    void Start()
    {
        Path path = Pathfinder.FindPath(grid, 0, 0, 5, 5);
        _ = actor.MoveAlongPath(path);
    }
}
