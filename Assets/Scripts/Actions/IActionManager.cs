using System.Collections;
using UnityEngine;

public interface IActionManager
{
    public void ExecuteSelectedAction(IActionArgs args);
    public void OnSelectedAcionCompleted();
}
