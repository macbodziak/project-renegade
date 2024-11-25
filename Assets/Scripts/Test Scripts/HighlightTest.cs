using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class HighlightTest : SerializedMonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [OdinSerialize][Required] IHighlighter highlighter;
    int stateID = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateID = (stateID + 1) % 3;
            highlighter.State = (HighlightState)stateID;
        }
    }
}
