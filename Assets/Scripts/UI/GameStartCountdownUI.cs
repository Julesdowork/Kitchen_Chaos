using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;

    private const string NUMBER_POPUP = "NumberPopup";

    private Animator _animator;
    private int _previousCountdownNumber;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.CountdownToStartTimer);
        _countdownText.text = countdownNumber.ToString();

        if (_previousCountdownNumber != countdownNumber)
        {
            _previousCountdownNumber = countdownNumber;
            _animator.SetTrigger(NUMBER_POPUP);
            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
