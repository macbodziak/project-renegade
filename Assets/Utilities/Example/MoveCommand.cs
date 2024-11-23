using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class MoveCommand : ICoroutineCommand
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

    public IEnumerator Execute()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
        while (gameObject.transform.position != targetPosition)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
    }
}
