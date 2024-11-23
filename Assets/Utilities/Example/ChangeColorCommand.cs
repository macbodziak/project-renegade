using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorCommand : Utilities.IInstantCommand
{
    GameObject gameObject;
    Color color;

    public ChangeColorCommand(GameObject _gameObject, Color _color)
    {
        gameObject = _gameObject;
        color = _color;
    }

    public void Execute()
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.material.color = color;
    }
}
