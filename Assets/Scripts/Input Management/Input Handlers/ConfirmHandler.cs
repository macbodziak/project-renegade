using System;
using UnityEngine;

public class ConfirmHandler : InputStateHandler
{
    public ConfirmHandler(LayerMask layerMask) : base(layerMask)
    {
    }

    public override void HandleInput()
    {

        if (selectAction.WasPerformedThisFrame() && IsMouseOverUI() == false)
        {
            OnMouseClicked();
        }

        if (cancelAction.WasPerformedThisFrame())
        {
            OnCancel();
        }
    }

    private void OnMouseClicked()
    {
        ConfirmArgs args = new ConfirmArgs(PlayerActionManager.Instance.SelectedUnit);
        PlayerActionManager.Instance.ExecuteSelectedAction(args);
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.CancelSelection();
    }
}
