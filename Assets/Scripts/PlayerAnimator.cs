using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player _playerMovement;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        _animator.SetBool(IS_WALKING, _playerMovement.IsWalking());
    }
}
