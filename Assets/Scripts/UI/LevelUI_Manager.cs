using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI_Manager : MonoBehaviour
{
    [SerializeField] Button _endTurnButton;

    private void Start()
    {
        RegisterEventHandlers();
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

    private void RegisterEventHandlers()
    {
        PlayerActionManager.Instance.ActionExecutionStartedEvent += HandleActionExecutionStarted;
        PlayerActionManager.Instance.ActionExecutionFinishedEvent += HandleActionExecutionFinished;
    }
}

