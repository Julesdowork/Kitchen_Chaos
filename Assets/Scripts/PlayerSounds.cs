using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player _player;
    private float _footstepTimer;
    private float _footstepTimerMax = 0.1f;

    void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footstepTimer -= Time.deltaTime;
        if (_footstepTimer < 0)
        {
            _footstepTimer = _footstepTimerMax;

            if (_player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepSound(_player.transform.position);
            }
        }
    }
}
