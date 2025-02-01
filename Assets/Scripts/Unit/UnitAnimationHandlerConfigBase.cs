using UnityEngine;

public abstract class UnitAnimationHandlerConfigBase : ScriptableObject
{
    public abstract int GetDanceHash();

    public abstract int GetRunHash();

    public abstract int GetMeleeAttackHash();

    public abstract int GetRangeAttackHash();
}