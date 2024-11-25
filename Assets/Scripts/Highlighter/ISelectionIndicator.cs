using UnityEngine;

public interface ISelectionIndicator
{
    public SelectionIndicatorState State { get; }
    public void SetActive(bool value);
    public void Review();
    public void StopReview();
}
