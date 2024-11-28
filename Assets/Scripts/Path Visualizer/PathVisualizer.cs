using Navigation;
using UnityEngine;

public abstract class PathVisualizer : MonoBehaviour
{
    protected Path _path;
    protected bool _isShowing;
    public bool IsShowing { get => _isShowing; }

    public abstract void Show();
    public abstract void UpdatePath(Path path);
    public abstract void Hide();
    public abstract void Clear();


}
