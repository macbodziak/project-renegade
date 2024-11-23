using System.Collections;

namespace Utilities
{
    // <summary>
    // Use this interface for commands that need to execute instantly, such as updating states.
    // queueing several instant commands will cause them to execute the same frame, as oposed to 
    // Coroutine Commands.
    // </summary>
    public interface IInstantCommand
    {
        public void Execute();
    }
}
