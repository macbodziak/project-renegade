using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Windows.Input;

namespace Utilities
{
    // <summary>
    // This class receives coroutines implementing the IInstantCommand or ICouroutineCommand 
    // interface to be later executed sequentially. 
    // It needs to receive a monobehaviour as a parameter, to which coroutines will be attached.
    // </summary>
    public class CommandQueue
    {

        MonoBehaviour monoBehaviour;
        Queue<object> commandQueue;

        public bool IsExecuting { get; private set; }
        bool cancelRequested;
        Coroutine currentCoroutine;

        public event EventHandler ExecutionCompletedEvent;

        public CommandQueue(MonoBehaviour _monoBehaviour)
        {
            monoBehaviour = _monoBehaviour;
            commandQueue = new Queue<object>();
            IsExecuting = false;
            cancelRequested = false;
        }

        // <summary>
        // This method starts executing queued routines 
        // </summary>
        public void Execute()
        {
            if (IsExecuting == true)
            {
                return;
            }

            monoBehaviour.StartCoroutine(ExecuteRoutine());
        }

        // <summary>
        // This is an Coroutine called internally to start execution of queued routines
        // </summary>
        private IEnumerator ExecuteRoutine()
        {
            object nextCommand = null;

            IsExecuting = true;
            cancelRequested = false;

            while (commandQueue.Count > 0)
            {
                nextCommand = commandQueue.Dequeue();
                if (nextCommand is Utilities.ICoroutineCommand coroutineCommand)
                {
                    currentCoroutine = monoBehaviour.StartCoroutine(coroutineCommand.Execute());
                    yield return currentCoroutine;
                }
                else if (nextCommand is Utilities.IInstantCommand instantCommand)
                {
                    instantCommand.Execute();
                }


                if (cancelRequested)
                {
                    commandQueue.Clear();
                    IsExecuting = false;
                    cancelRequested = false;
                    ExecutionCompletedEvent?.Invoke(this, EventArgs.Empty);
                    yield break;
                }
            }
            commandQueue.Clear();
            IsExecuting = false;
            ExecutionCompletedEvent?.Invoke(this, EventArgs.Empty);
        }

        // <summary>
        // This method orders the Queue to stop executing routines once the currently executing routine finishes execution
        // </summary>
        public void Cancel()
        {
            if (IsExecuting)
            {
                cancelRequested = true;
            }
            else
            {
                commandQueue.Clear();
            }
        }

        // <summary>
        // This method orders the Queue to stop executing routines immediately without waiting for the currently executing 
        // to finish
        // </summary>
        public void Stop()
        {
            if (currentCoroutine != null)
            {
                monoBehaviour.StopCoroutine(currentCoroutine);
            }
            commandQueue.Clear();
            currentCoroutine = null;

        }

        // <summary>
        // Removes all queued routines from the queue if the queue has not started execution.
        // This is not intended to be called during execution.
        // </summary>
        public void Clear()
        {
            if (IsExecuting == true)
            {
                return;
            }

            commandQueue.Clear();
            cancelRequested = false;
        }

        // <summary>
        // Add a routine to the queue to be later executed
        // </summary>
        public void Add(ICoroutineCommand command)
        {
            commandQueue.Enqueue(command);
        }

        public void Add(IInstantCommand command)
        {
            commandQueue.Enqueue(command);
        }


    }
}