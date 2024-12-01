using Unity.VisualScripting;
using UnityEngine;

public class InputBlockedHandler : InputStateHandler
{
    public InputBlockedHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void OnEnter()
    {
        LevelManager.Instance.HideWalkableArea();
    }

    public override void HandleInput()
    {
    }
}
