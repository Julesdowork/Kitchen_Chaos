using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;
    private bool _isGamePaused = false;

    public float CountdownToStartTimer => _countdownToStartTimer;
    public float GamePlayingTimerNormalized => 1 - _gamePlayingTimer / _gamePlayingTimerMax;

    void Awake()
    {
        Instance = this;

        _state = State.WaitingToStart;
    }

    void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
        GameInput.Instance.OnInteract += GameInput_OnInteract;
    }

    private void GameInput_OnInteract(object sender, EventArgs e)
    {
        if (_state == State.WaitingToStart)
        {
            _state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    void Update()
    {
        switch (_state)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer < 0)
                {
                    _state = State.GamePlaying;
                    _gamePlayingTimer = _gamePlayingTimerMax;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer -= Time.deltaTime;
                if (_gamePlayingTimer < 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return _state == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return _state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return _state == State.GameOver;
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;
        if (_isGamePaused)
        {
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
