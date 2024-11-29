using UnityEngine;


public interface IAction
{
    public void Execute(IActionManager actionManager, IActionArgs args);
    public bool InProgress();

}

