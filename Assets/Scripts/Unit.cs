using UnityEngine;

public class Unit : MonoBehaviour
{

    Animator _animator;
    [SerializeField]
    bool _isPlayer = false;

    public bool IsPlayer { get => _isPlayer; }

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }


}
