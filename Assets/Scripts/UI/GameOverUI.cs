using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _recipesDeliveredText;
    //[SerializeField] private Button _retryButton;
    [SerializeField] private Button _mainMenuButton;

    private float _waitForButtonsTimer;
    private float _waitForButtonsTimerMax = 4f;
    private bool _showingButtons;

    void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });

        _mainMenuButton.gameObject.SetActive(false);
        //_retryButton.gameObject.SetActive(false);
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        Hide();
    }

    void Update()
    {
        if (_showingButtons) return;

        if (_waitForButtonsTimer >= _waitForButtonsTimerMax)
        {
            ShowButtons();
            _showingButtons = true;
        }
        else
        {
            _waitForButtonsTimer += Time.deltaTime;
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            
            _recipesDeliveredText.text = DeliveryManager.Instance.SuccessfulRecipesAmount.ToString();
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

    private void ShowButtons()
    {
        //_retryButton.gameObject.SetActive(true);
        //_retryButton.Select();
        _mainMenuButton.gameObject.SetActive(true);
        _mainMenuButton.Select();
    }
}
