using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Button _closeButton;

    void Awake()
    {
        _closeButton.onClick.AddListener(Hide);
    }

    void Start()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame += GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted += GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed += GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted += GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed += GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed += GameLobby_OnQuickJoinFailed;

        Hide();
    }

    void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailedToJoinGame -= GameMultiplayer_OnFailedToJoinGame;
        GameLobby.Instance.OnCreateLobbyStarted -= GameLobby_OnCreateLobbyStarted;
        GameLobby.Instance.OnCreateLobbyFailed -= GameLobby_OnCreateLobbyFailed;
        GameLobby.Instance.OnJoinStarted -= GameLobby_OnJoinStarted;
        GameLobby.Instance.OnJoinFailed -= GameLobby_OnJoinFailed;
        GameLobby.Instance.OnQuickJoinFailed -= GameLobby_OnQuickJoinFailed;
    }

    private void GameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find a Lobby to Quick Join!");
    }

    private void GameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join Lobby!");
    }

    private void GameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby...");
    }

    private void GameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
    }

    private void GameLobby_OnCreateLobbyStarted(object sender, System.EventArgs e)
    {
        ShowMessage("Creating Lobby...");
    }

    private void GameMultiplayer_OnFailedToJoinGame(object sender, System.EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        _messageText.text = message;
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
