using UnityEngine;

public class InputBlockedHandler : InputStateHandler
{
    public InputBlockedHandler(LayerMask unitLayerMask) : base(unitLayerMask)
    {
    }

    public override void HandleInput()
    {
    }
}
