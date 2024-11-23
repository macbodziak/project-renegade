using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Utilities;

public class TestScript : MonoBehaviour
{
    CommandQueue queue;
    [SerializeField] GameObject agent;
    [SerializeField] Transform point_1;
    [SerializeField] Transform point_2;
    [SerializeField] Transform point_3;
    [SerializeField] Transform point_4;
    [SerializeField] float speed_1;
    [SerializeField] float speed_2;
    [SerializeField] float speed_3;
    [SerializeField] float speed_4;

    void Start()
    {
        queue = new CommandQueue(this);
        queue.ExecutionCompletedEvent += OnExecutionFinished;
        queue.Add(new ChangeColorCommand(agent, Color.magenta));
        queue.Add(new MoveCommand(agent, point_1.position, speed_1));
        queue.Add(new MoveCommand(agent, point_2.position, speed_2));
        queue.Add(new MoveCommand(agent, point_3.position, speed_3));
        queue.Add(new ChangeColorCommand(agent, Color.red));
        queue.Add(new MoveCommand(agent, point_4.position, speed_4));
        queue.Add(new ChangeColorCommand(agent, Color.yellow));
        queue.Add(new ChangeColorCommand(agent, Color.blue));
        queue.Execute();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            queue.Cancel();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            queue.Add(new MoveCommand(agent, point_1.position, speed_3));
            queue.Add(new ChangeColorCommand(agent, Color.cyan));
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            queue.Stop();
        }
    }

    void OnExecutionFinished(object sender, EventArgs evt)
    {
        Debug.Log("OnExecutionFinished event handled");
    }
}
