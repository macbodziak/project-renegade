using System;
using TMPro;
using UnityEngine;

public class TurnManager : PersistentSingelton<TurnManager>
{
    bool _isPlayerTurn;

    public event Action<bool> TurnEndedEvent;


    protected override void InitializeOnAwake()
    {
        _isPlayerTurn = true;
    }

    public void EndTurn()
    {
        Debug.Log($"<color=#c0ff8b>On End Turn</color>");
        if (_isPlayerTurn)
        {
            StartEnemyTurn();
            _isPlayerTurn = false;
        }
        else
        {
            StartPlayerTurn();
            _isPlayerTurn = true;
        }

        TurnEndedEvent?.Invoke(_isPlayerTurn);
    }

    private void StartPlayerTurn()
    {
        RefreshPlayerUnits();
        if (PlayerActionManager.Instance.SelectedUnit != null)
        {
            PlayerActionManager.Instance.SetSelectedAbility(PlayerActionManager.Instance.SelectedUnit.MoveAbility);
            //TODO reset UI ability panel
        }
        else
        {
            InputManager.Instance.SetState(InputManager.State.SelectUnit);
        }
    }

    private void RefreshPlayerUnits()
    {
        LevelManager.Instance.NullifyPlayerWalkableAreas();
        foreach (var unit in LevelManager.Instance.PlayerUnits)
        {
            unit.RefreshOnNewTurn();
        }
    }

    private void StartEnemyTurn()
    {
        InputManager.Instance.SetState(InputManager.State.InputBlocked);
        EmulateEnemyTurn();
    }


    //TODO - remove this once enemy AI is implemented
    private async void EmulateEnemyTurn()
    {
        Debug.Log($"<color=#8bc5ff>Simulating Enemy Turn...</color>");
        await Awaitable.WaitForSecondsAsync(1.75f);
        Debug.Log($"<color=#8bc5ff>Enemy Turn Ended</color>");
        EndTurn();
    }

}
