using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ICommand = Utilities.ICommand;

public class DanceCommand : ICommand
{
    private Unit _unit;
    private float _duration;

    public DanceCommand(Unit unit, float duration)
    {
        _duration = duration;
        _unit = unit;
    }

    public async Task Execute(CancellationToken token)
    {
        float time_start = Time.realtimeSinceStartup;
        float time_now = Time.realtimeSinceStartup;

        //todo command should not be couppled to the units animator
        _unit.animator.SetBool("Dancing", true);
        while (time_now - time_start < _duration)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }
            await Awaitable.NextFrameAsync();
            time_now = Time.realtimeSinceStartup;
        }
        _unit.animator.SetBool("Dancing", false);
    }
}
