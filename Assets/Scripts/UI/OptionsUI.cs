using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] private Button _soundEffectsButton;
    [SerializeField] private Button _musicButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _soundEffectsText;
    [SerializeField] private TextMeshProUGUI _musicText;

    void Awake()
    {
        Instance = this;

        _soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        _closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    void Start()
    {
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;

        UpdateVisual();

        Hide();
    }

    private void GameManager_OnGameUnpaused(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        _soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.Volume * 10f);
        _musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.Volume * 10f);
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
