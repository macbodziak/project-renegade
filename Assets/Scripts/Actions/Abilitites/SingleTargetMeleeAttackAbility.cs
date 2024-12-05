
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetMeleeAttackAbility", menuName = "Abilitites/Single Target Melee Attack")]
public class SingleTargetMeleeAttackAbility : Ability
{
    public override InputManager.State InputState => InputManager.State.SelectSingleMeleeTarget;

    public override async void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    {
        SingleMeleeTargetArgs args = abilityArgs as SingleMeleeTargetArgs;
        // Unit t;
        Task faceTowardsTask = args.AttackingUnit.actor.FaceTowards(args.TargetUnit.WorldPosition);

        while (faceTowardsTask.IsCompleted == false)
        {
            await Awaitable.NextFrameAsync();
        }
        //TODO here should be more logic, also, we should decouble action from animator 
        args.AttackingUnit.animator.SetTrigger("AttackMelee");
        await Awaitable.WaitForSecondsAsync(0.2f);
        actionManager.OnSelectedAcionCompleted();
    }
}