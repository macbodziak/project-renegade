using UnityEngine;

namespace Game
{
    public interface IAction
    {
        public void Execute(IActionManager actionManager, IActionArgs args);
        public bool InProgress();

    }
}
