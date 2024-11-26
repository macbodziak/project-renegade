using Navigation;
using UnityEditor.SceneManagement;
using UnityEngine;

public class WalkableAreaVisualizerTest : MonoBehaviour
{
    [SerializeField]
    Actor actor;
    [SerializeField]
    NavGrid grid;
    public int budget;
    [SerializeField]
    MosaicVisualizer mv;

    void Start()
    {
        // WalkableArea area = Pathfinder.FindWalkableArea(grid, actor.NodeIndex, budget);
        // mv.UpdateWalkableArea(area);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WalkableArea area = Pathfinder.FindWalkableArea(grid, actor.NodeIndex, budget);
            mv.UpdateWalkableArea(area);
            mv.Show();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            mv.Hide();
        }
    }
}
