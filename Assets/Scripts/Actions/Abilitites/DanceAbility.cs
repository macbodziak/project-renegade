using UnityEngine;

[CreateAssetMenu(fileName = "DanceAbility", menuName = "Abilitites/Dance")]
public class DanceAbility : Ability
{

    public override IAction GetAction()
    {
        return new DanceAction();
    }
}
