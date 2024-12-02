using UnityEngine;


public interface IAction
{
    public void Execute(IActionManager actionManager, ActionArgs args);
    public bool InProgress();

}

