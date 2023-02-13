using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const string MUSIC_VOLUME_KEY = "MusicVolume";

    private AudioSource _audioSource;
    private float _volume = 0.3f;

    public float Volume => _volume;

    void Awake()
    {
        Instance = this;

        _audioSource= GetComponent<AudioSource>();

        _volume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.3f);
        _audioSource.volume = _volume;
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        if (_volume > 1f)
        {
            _volume = 0;
        }
        _audioSource.volume = _volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _volume);
        PlayerPrefs.Save();
    }
}
