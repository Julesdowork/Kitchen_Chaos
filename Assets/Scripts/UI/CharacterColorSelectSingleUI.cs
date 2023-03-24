using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterColorSelectSingleUI : MonoBehaviour
{
    [SerializeField] private int _colorId;
    [SerializeField] private Image _image;
    [SerializeField] private GameObject _selectedGameObject;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.ChangePlayerColor(_colorId);
        });
    }

    void Start()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += GameMultiplayer_OnPlayerDataNetworkListChanged;
        _image.color = GameMultiplayer.Instance.GetPlayerColor(_colorId);
        UpdateIsSelected();
    }

    void OnDestroy()
    {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= GameMultiplayer_OnPlayerDataNetworkListChanged;
    }

    private void GameMultiplayer_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e)
    {
        UpdateIsSelected();
    }

    private void UpdateIsSelected()
    {
        if (GameMultiplayer.Instance.GetPlayerData().colorId == _colorId)
        {
            _selectedGameObject.SetActive(true);
        }
        else
        {
            _selectedGameObject.SetActive(false);
        }
    }
}
