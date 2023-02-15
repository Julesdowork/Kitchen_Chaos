using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;

    private const string IS_FLASHING = "IsFlashing";

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        _animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = _stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;

        _animator.SetBool(IS_FLASHING, show);
    }
}
