using Navigation;
using UnityEngine;

public class NullHandler : InputStateHandler
{
    public NullHandler(LayerMask layerMask) : base(layerMask)
    {
    }

    public override string PromptText => "";

    public override void HandleInput()
    {

    }
}