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
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        actor.MoveAlongPath(path);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }
}
