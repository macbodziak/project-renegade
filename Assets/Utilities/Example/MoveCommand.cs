using System.Threading;
using UnityEngine;
using Utilities;

public class MoveCommand : ICommand
{
    GameObject gameObject;
    Vector3 targetPosition;
    float speed;

    public MoveCommand(GameObject _gameObject, Vector3 _position, float _speed)
    {
        gameObject = _gameObject;
        targetPosition = _position;
        speed = _speed;
    }

    public async Awaitable Execute(CancellationToken cancellationToken)
    {
        Debug.Log($"Executing Move command at frame <color=#ffa08b>{Time.frameCount}</color>");
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
        while (gameObject.transform.position != targetPosition)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
            await Awaitable.NextFrameAsync();
        }
    }
}
