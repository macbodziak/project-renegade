using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;

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
        }
    }

    private void HandleActionExecutionStarted(object sender, EventArgs eventArgs)
    {
        Debug.Log($"<color=orange>HandleActionExecutionStarted</color>");
        _endTurnButton.interactable = false;
    }

    private void HandleActionExecutionFinished(object sender, EventArgs eventArgs)
    {
        Debug.Log($"<color=orange>HandleActionExecutionFinished</color>");
        _endTurnButton.interactable = true;
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
    }

    private void EnablePlayerInteraction()
    {
        _endTurnButton.interactable = true;
    }

    private void RegisterEventHandlers()
    {
        PlayerActionManager.Instance.ActionExecutionStartedEvent += HandleActionExecutionStarted;
        PlayerActionManager.Instance.ActionExecutionFinishedEvent += HandleActionExecutionFinished;

        TurnManager.Instance.TurnEndedEvent += HandleTurnEndedEvent;
    }
}

