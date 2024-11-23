using System.Collections;

namespace Utilities
{
    // <summary>
    // Use this interface for commands that need to execute over time, such as movement etc.
    // When one command finishes execution, the next will start the next frame. 
    // </summary>
    public interface ICoroutineCommand
    {
        public IEnumerator Execute();
    }
}
