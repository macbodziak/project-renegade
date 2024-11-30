using Sirenix.OdinInspector;
using UnityEngine;

public class ActionExecutionTest : MonoBehaviour
{
    public float _danceDuration = 1.5f;
    [InfoBox("Select a Unit in play mode before executing test")]
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Button("Execute Dance Action")]
    public void TestDanceAction()
    {
        Unit _unit = PlayerActionManager.Instance.SelectedUnit;
        if (_unit == null)
        {
            Debug.Log("No Unit Selected");
            return;
        }
        PlayerActionManager.Instance.SetSelectedAction(new DanceAction());
        PlayerActionManager.Instance.ExecuteSelectedAction(new DanceActionArgs(_unit, _danceDuration));
    }
}
