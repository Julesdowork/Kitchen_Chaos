using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _readyButton;
    [SerializeField] private TextMeshProUGUI _lobbyNameText;
    [SerializeField] private TextMeshProUGUI _lobbyCodeText;

    void Awake()
    {
        _mainMenuButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.LeaveLobby();
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
        _readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
        });
    }

    void Start()
    {
        Lobby lobby = GameLobby.Instance.GetLobby();

        _lobbyNameText.text = "Lobby Name: " + lobby.Name;
        _lobbyCodeText.text = "Lobby Code: " + lobby.LobbyCode;
    }
}
