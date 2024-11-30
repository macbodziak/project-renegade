using System;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static TurnManager _instance;
    bool _isPlayerTurn;

    public event EventHandler<bool> TurnEndedEvent;
    public static TurnManager Instance { get => _instance; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
            InitializeOnAwake();
        }
    }

    private void InitializeOnAwake()
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

        TurnEndedEvent?.Invoke(this, _isPlayerTurn);
    }

    private void StartPlayerTurn()
    {
        RefreshPlayerUnits();
        InputManager.Instance.SetState(InputManager.State.SelectUnit);
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
