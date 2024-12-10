using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Utilities;

public class FaceTowardsCommand : ICommand
{
    private Unit _unit;
    private Vector3 _targetPoint;

    public FaceTowardsCommand(Unit unit, Vector3 targetPoint)
    {
        _unit = unit;
        _targetPoint = targetPoint;
    }

    public Task Execute(CancellationToken token)
    {
        //ignore cancellation token
        return _unit.FaceTowards(_targetPoint);
    }
}