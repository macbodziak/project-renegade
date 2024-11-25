using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public class HighlightTest : SerializedMonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField][Required] List<SelectionIndicator> indicators;

    void Awake()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var indicator in indicators)
            {
                indicator.IsActive = !indicator.IsActive;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            foreach (var indicator in indicators)
            {
                indicator.IsReviewed = !indicator.IsReviewed;
            }
        }
    }
}
