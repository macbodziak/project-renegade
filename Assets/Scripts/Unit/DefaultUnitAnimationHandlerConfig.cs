using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "DefaultUnitAnimationHandlerConfig",
    menuName = "Unit Animation Handler Config/DefaultUnitAnimationHandlerConfig")]
public class DefaultUnitAnimationHandlerConfig : UnitAnimationHandlerConfigBase
{
    [Title("Animation Parameter names")] [SerializeField]
    private List<string> _meleeAttack;

    [SerializeField] private List<string> _rangeAttack;

    [SerializeField] private List<string> _run;

    [SerializeField] private List<string> _dance;

    private List<int> _danceHashes;

    private List<int> _runHashes;

    private List<int> _meleeAttackHashes;

    private List<int> _rangeAttackHashes;

    public override int GetDanceHash()
    {
        return _danceHashes[Random.Range(0, _danceHashes.Count)];
    }

    public override int GetRunHash()
    {
        return _runHashes[Random.Range(0, _runHashes.Count)];
    }

    public override int GetMeleeAttackHash()
    {
        return _meleeAttackHashes[Random.Range(0, _meleeAttackHashes.Count)];
    }

    public override int GetRangeAttackHash()
    {
        return _rangeAttackHashes[Random.Range(0, _rangeAttackHashes.Count)];
    }

    private void OnEnable()
    {
        Debug.Log("DefaultUnitAnimationHandlerConfig.OnEnable");
        _runHashes = CreateHashesFromNames(_run);
        _danceHashes = CreateHashesFromNames(_dance);
        _meleeAttackHashes = CreateHashesFromNames(_meleeAttack);
        _rangeAttackHashes = CreateHashesFromNames(_rangeAttack);
    }

    private static List<int> CreateHashesFromNames(List<string> names)
    {
        Assert.IsTrue(names.Count > 0);
        List<int> hashes = new List<int>(names.Count);
        foreach (var name in names)
        {
            hashes.Add(Animator.StringToHash(name));
        }

        return hashes;
    }
}