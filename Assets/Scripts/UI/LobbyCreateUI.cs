using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _createPublicButton;
    [SerializeField] private Button _createPrivateButton;
    [SerializeField] private TMP_InputField _lobbyNameInputField;

    void Awake()
    {
        _createPublicButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.CreateLobby(_lobbyNameInputField.text, false);
        });
        _createPrivateButton.onClick.AddListener(() =>
        {
            GameLobby.Instance.CreateLobby(_lobbyNameInputField.text, true);
        });
        _closeButton.onClick.AddListener(() => Hide());
    }

    void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
