using System.Threading;

using UnityEngine;

public class ChangeColorCommand : Utilities.ICommand
{
    GameObject gameObject;
    Color color;

    public ChangeColorCommand(GameObject _gameObject, Color _color)
    {
        gameObject = _gameObject;
        color = _color;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Awaitable Execute(CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        Debug.Log($"Executing Change Color command at frame <color=#c78bff>{Time.frameCount}</color>");
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.material.color = color;
    }
}
