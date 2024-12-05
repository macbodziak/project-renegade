using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;
    [SerializeField] AbilitiesPanel _abilitiesPanel;
    [SerializeField] TextMeshProUGUI _promptText;

    private void Start()
    {
        RegisterEventHandlers();

        _endTurnButton.onClick.AddListener(TurnManager.Instance.EndTurn);

        LateStart();
    }

    private async void LateStart()
    {
        await Awaitable.EndOfFrameAsync();
        _promptText.text = "";
        _promptText.GetComponent<UIBlinker>()?.StartBlinking();
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

        InputManager.Instance.InputStateChangedEvent += HandleInputStateChanged;

        TurnManager.Instance.TurnEndedEvent += HandleTurnEndedEvent;
    }

    private void HandleInputStateChanged()
    {
        string newText = InputManager.Instance.GetInputPromptText();
        _promptText.text = newText;
    }


    private void HandleActionExecutionStarted()
    {
        DisablePlayerInteraction();
    }

    private void HandleActionExecutionFinished()
    {
        EnablePlayerInteraction();
        _abilitiesPanel.Reset();
    }

    private void HandleTurnEndedEvent(bool isPlayerTurn)
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


    private void HandleUnitSelectionChanged(SelectedUnitChangedEventArgs e)
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

