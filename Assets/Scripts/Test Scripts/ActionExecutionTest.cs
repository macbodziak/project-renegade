using Sirenix.OdinInspector;
using UnityEngine;

public class ActionExecutionTest : MonoBehaviour
{
    public float _duration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Button("Execute Dance Action with Selected Unit")]
    public void TestDanceAction()
    {
        Unit _unit = PlayerActionManager.Instance.SelectedUnit;
        if (_unit == null)
        {
            Debug.Log("No Unit Selected");
            return;
        }
        PlayerActionManager.Instance.SetSelectedAction(new DanceAction());
        PlayerActionManager.Instance.ExecuteSelectedAction(new DanceActionArgs(_unit, _duration));
    }
}
