using UnityEngine;

[CreateAssetMenu(fileName = "DanceAbility", menuName = "Abilitites/Dance")]
public class DanceAbility : Ability
{
    [SerializeField] float _duration = 2.5f;

    public float Duration { get => _duration; }

    public override InputManager.State InputState => InputManager.State.Confirm;

    public async override void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    {
        ConfirmArgs args = abilityArgs as ConfirmArgs;
        Unit unit = args.Unit;

        unit.animator.SetBool("Dancing", true);
        await Awaitable.WaitForSecondsAsync(_duration);
        unit.animator.SetBool("Dancing", false);
        actionManager.OnSelectedAcionCompleted();
    }
}
