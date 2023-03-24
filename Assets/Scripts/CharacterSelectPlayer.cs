using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayer : MonoBehaviour
{
    [SerializeField] private int _playerIndex;
    [SerializeField] private GameObject _readyGameObject;
    [SerializeField] private PlayerVisual _playerVisual;
    [SerializeField] private Button _kickButton;

    private void Awake()
    {
        _kickButton.onClick.AddListener(() =>
        {
            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);
            GameMultiplayer.Instance.KickPlayer(playerData.clientId);
        });
    }

    void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_OnPlayerDataNetworkListChanged;
        CharacterSelectReady.Instance.OnReadyChanged += CharacterSelectReady_OnReadyChanged;

        _kickButton.gameObject.SetActive(NetworkManager.Singleton.IsServer);

        UpdatePlayer();
    }

    void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayer_OnPlayerDataNetworkListChanged;
    }

    private void CharacterSelectReady_OnReadyChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void GameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (GameMultiplayer.Instance.IsPlayerIndexConnected(_playerIndex))
        {
            Show();

            PlayerData playerData = GameMultiplayer.Instance.GetPlayerDataFromPlayerIndex(_playerIndex);

            _readyGameObject.SetActive(CharacterSelectReady.Instance.IsPlayerReady(playerData.clientId));
            
            _playerVisual.SetPlayerColor(GameMultiplayer.Instance.GetPlayerColor(playerData.colorId));
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
