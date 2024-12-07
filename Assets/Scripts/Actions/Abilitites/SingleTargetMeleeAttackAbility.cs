
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetMeleeAttackAbility", menuName = "Abilitites/Single Target Melee Attack")]
public class SingleTargetMeleeAttackAbility : Ability
{
    public override InputManager.State InputState => InputManager.State.SelectSingleMeleeTarget;

    public override async void Execute(IActionManager actionManager, AbilityArgs abilityArgs)
    {
        SingleMeleeTargetArgs args = abilityArgs as SingleMeleeTargetArgs;

        if (args.Path != null)
        {
            args.AttackingUnit.animator.SetBool("Running", true);
            await args.AttackingUnit.MoveAlongPath(args.Path);
            args.AttackingUnit.animator.SetBool("Running", false);
        }

        Task faceTowardsTask = args.AttackingUnit.actor.FaceTowardsAsync(args.TargetUnit.WorldPosition);

        while (faceTowardsTask.IsCompleted == false)
        {
            await Awaitable.NextFrameAsync();
        }
        //TODO here should be more logic, also, we should decouble action from animator 
        Debug.Log("<color=#ffa08b>TO DO</color> add  logic here, such as HP reduction, AP reduction etc.");
        args.AttackingUnit.animator.SetTrigger("AttackMelee");
        await Awaitable.WaitForSecondsAsync(0.2f);
        actionManager.OnSelectedAcionCompleted();
    }
}