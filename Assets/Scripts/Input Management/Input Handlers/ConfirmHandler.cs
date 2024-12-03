using System;
using UnityEngine;

public class ConfirmHandler : InputStateHandler
{
    public ConfirmHandler(LayerMask layerMask) : base(layerMask)
    {
    }

    public override void HandleInput()
    {
        if (selectAction.WasPerformedThisFrame())
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
        PlayerActionManager.Instance.actionArgs.ActingUnit = PlayerActionManager.Instance.SelectedUnit;
        PlayerActionManager.Instance.ExecuteSelectedAction();
    }

    private void OnCancel()
    {
        PlayerActionManager.Instance.CancelSelection();
    }
}
