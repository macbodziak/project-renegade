using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using ICommand = Utilities.ICommand;

public class DanceCommand : ICommand
{
    private readonly Unit _unit;
    private readonly float _duration;

    public DanceCommand(Unit unit, float duration)
    {
        _duration = duration;
        _unit = unit;
    }

    public async Task Execute(CancellationToken token)
    {
        float time_start = Time.realtimeSinceStartup;
        float time_now = Time.realtimeSinceStartup;

        _unit.AnimationHandler?.PlayDanceAnimation();
        while (time_now - time_start < _duration)
        {
            if (token.IsCancellationRequested)
            {
                return;
            }

            await Awaitable.NextFrameAsync(token);
            time_now = Time.realtimeSinceStartup;
        }

        _unit.AnimationHandler?.StopDanceAnimation();
    }
}