using Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

public class LinePathVisualizer : PathVisualizer
{
    LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

#if DEBUG
        Debug.Assert(_lineRenderer != null, "LinePathVisualizer is missing LineRenderer");
#endif
    }

    public override void Clear()
    {
        _path = null;
        _lineRenderer.positionCount = 0;
    }

    public override void Hide()
    {
        _lineRenderer.enabled = false;
    }

    public override void Show()
    {
        _lineRenderer.enabled = true;
    }

    public override void UpdatePath(Path path)
    {
        _path = path;
        if (_path == null || path.Count == 0)
        {
            return;
        }

        _lineRenderer.positionCount = path.Count;
        _lineRenderer.SetPositions(path.worldPositions);
    }
}
