using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button _playMultiplayerButton;
    [SerializeField] private Button _playSingleplayerButton;
    [SerializeField] private Button _quitButton;

    void Awake()
    {
        _playMultiplayerButton.onClick.AddListener(() =>
        {
            GameMultiplayer.playMultiplayer = true;
            Loader.Load(Loader.Scene.Lobby);
        });
        _playSingleplayerButton.onClick.AddListener(() =>
        {
            GameMultiplayer.playMultiplayer = false;
            Loader.Load(Loader.Scene.Lobby);
        });
        _quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
}
