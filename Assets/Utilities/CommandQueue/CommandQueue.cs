using System.Collections.Generic;
using System;
using System.Threading;

namespace Utilities
{
    // <summary>
    // This class receives coroutines implementing the IInstantCommand or ICouroutineCommand 
    // interface to be later executed sequentially. 
    // It needs to receive a monobehaviour as a parameter, to which coroutines will be attached.
    // </summary>
    public class CommandQueue
    {

        Queue<ICommand> _commandQueue;
        private CancellationTokenSource _tokenSource;

        public bool IsExecuting { get; private set; }
        bool _stopRequested;

        public event EventHandler ExecutionCompletedEvent;

        public CommandQueue()
        {
            _commandQueue = new();
            IsExecuting = false;
            _stopRequested = false;
            _tokenSource = new();
        }

        // <summary>
        // This method starts executing queued commands 
        // </summary>
        public async void Execute()
        {
            if (IsExecuting == true)
            {
                return;
            }

            ICommand nextCommand = null;

            IsExecuting = true;
            _stopRequested = false;

            while (_commandQueue.Count > 0)
            {
                nextCommand = _commandQueue.Dequeue();

                await nextCommand.Execute(_tokenSource.Token);

                if (_stopRequested)
                {
                    _stopRequested = false;
                    break;
                }
            }
            _commandQueue.Clear();
            IsExecuting = false;
            ExecutionCompletedEvent?.Invoke(this, EventArgs.Empty);
        }

        // <summary>
        // This method orders the Queue to stop executing command once the currently executing command finishes execution
        // </summary>
        public void Stop()
        {
            if (IsExecuting)
            {
                _stopRequested = true;
            }
            else
            {
                _commandQueue.Clear();
            }
        }

        // <summary>
        // This method orders the Queue to stop executing commands immediately and cancels the currently
        // executing task
        // </summary>
        public void Cancel()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = new();
            _commandQueue.Clear();
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

            _commandQueue.Clear();
            _stopRequested = false;
        }

        // <summary>
        // Add a Command to the queue to be later executed
        // </summary>
        public void Add(ICommand command)
        {
            _commandQueue.Enqueue(command);
        }

        ~CommandQueue()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
    }
}