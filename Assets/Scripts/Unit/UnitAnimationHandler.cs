using UnityEngine;
using UnityEngine.Assertions;

public class UnitAnimationHandler : MonoBehaviour
{
    private Animator _animator;

    //cache hashes of parameters that are not triggers 
    private int _currentRunHash;

    private int _currentDanceHash;

    private bool _commandInProgress;


    [SerializeField] UnitAnimationHandlerConfigBase _config;

    public bool CommandInProgress => _commandInProgress;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();

        Assert.IsNotNull(_animator);
        Assert.IsNotNull(_config);
    }

    public void PlayDanceAnimation()
    {
        _currentDanceHash = _config.GetDanceHash();
        _animator.SetBool(_currentDanceHash, true);
    }

    public void StopDanceAnimation()
    {
        _animator.SetBool(_currentDanceHash, false);
    }

    public void PlayRunningAnimation()
    {
        _currentRunHash = _config.GetRunHash();
        _animator.SetBool(_currentRunHash, true);
    }

    public void StopRunningAnimation()
    {
        _animator.SetBool(_currentRunHash, false);
    }

    public void TriggerMeleeAttackAnimation()
    {
        _animator.SetTrigger(_config.GetMeleeAttackHash());
        _commandInProgress = true;
    }
}