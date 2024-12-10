using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Utilities
{
    // <summary>
    // Commands that should run asynchronously should have await statements 
    // e.g. await Awaitable.NextFrameAsync();
    // Commands that should run synchronously, i.e. finish in one frame should not 
    // have any await statements
    // </summary>
    public interface ICommand
    {
        public Task Execute(CancellationToken token);
    }
}