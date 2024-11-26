using Navigation;
using UnityEngine;

public abstract class WalkableAreaVisualizer : MonoBehaviour
{
    protected WalkableArea _area;
    protected bool _isShowing;
    protected float _tileSize = 1f;

    public bool IsShowing { get => _isShowing; }

    public abstract void Show();
    public abstract void Hide();
    public abstract void UpdateWalkableArea(WalkableArea area);
    public abstract void Initialize(float tileSize);

}
