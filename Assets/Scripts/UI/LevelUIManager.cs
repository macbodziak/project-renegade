using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;
    [SerializeField] AbilitiesPanel _abilitiesPanel;

    private void Start()
    {
        RegisterEventHandlers();

        _endTurnButton.onClick.AddListener(TurnManager.Instance.EndTurn);
    }

    private void OnDestroy()
    {
        if (PlayerActionManager.Instance != null)
        {
            PlayerActionManager.Instance.ActionExecutionStartedEvent -= HandleActionExecutionStarted;
            PlayerActionManager.Instance.ActionExecutionFinishedEvent -= HandleActionExecutionFinished;
            PlayerActionManager.Instance.UnitSelectionChangedEvent -= HandleUnitSelectionChanged;
        }
    }


    private void RegisterEventHandlers()
    {
        PlayerActionManager.Instance.ActionExecutionStartedEvent += HandleActionExecutionStarted;
        PlayerActionManager.Instance.ActionExecutionFinishedEvent += HandleActionExecutionFinished;
        PlayerActionManager.Instance.UnitSelectionChangedEvent += HandleUnitSelectionChanged;

        TurnManager.Instance.TurnEndedEvent += HandleTurnEndedEvent;
    }

    private void HandleActionExecutionStarted(object sender, EventArgs eventArgs)
    {
        DisablePlayerInteraction();
    }

    private void HandleActionExecutionFinished(object sender, EventArgs eventArgs)
    {
        EnablePlayerInteraction();
        _abilitiesPanel.Reset();
    }

    private void HandleTurnEndedEvent(object sender, bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            EnablePlayerInteraction();
        }
        else
        {
            DisablePlayerInteraction();
        }
    }

    private void DisablePlayerInteraction()
    {
        _endTurnButton.interactable = false;
        _abilitiesPanel.interactable = false;
    }

    private void EnablePlayerInteraction()
    {
        _endTurnButton.interactable = true;
        _abilitiesPanel.interactable = true;
    }


    private void HandleUnitSelectionChanged(object sender, SelectedUnitChangedEventArgs e)
    {
        if (e.CurrentUnit != null)
        {
            _abilitiesPanel.UpdateAbilities(e.CurrentUnit.Abilities);
            _abilitiesPanel.Show();
        }
        else
        {
            _abilitiesPanel.Hide();
        }
    }

}

