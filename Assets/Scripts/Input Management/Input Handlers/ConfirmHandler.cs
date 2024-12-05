using System;
using UnityEngine;

public class ConfirmHandler : InputStateHandler
{
    public override string PromptText => "click to confirm";

    public ConfirmHandler(LayerMask layerMask) : base(layerMask)
    {
    }

    public override void HandleInput()
    {

        if (_selectAction.WasPerformedThisFrame() && IsMouseOverUI() == false)
        {
            OnMouseClicked();
        }

        if (_cancelAction.WasPerformedThisFrame())
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
