using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO _audioClipRefsSo;

    private const string SOUND_EFFECTS_VOLUME_KEY = "SoundEffectsVolume";

    private float _volume = 1f;

    public float Volume => _volume;

    void Awake()
    {
        Instance = this;

        _volume = PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME_KEY, 1f);
    }

    void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = (TrashCounter)sender;
        PlaySound(_audioClipRefsSo.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = (BaseCounter)sender;
        PlaySound(_audioClipRefsSo.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        PlaySound(_audioClipRefsSo.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = (CuttingCounter)sender;
        PlaySound(_audioClipRefsSo.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipRefsSo.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(_audioClipRefsSo.deliverySuccess, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume=1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volumeMultiplier=1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volumeMultiplier * _volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume=1f)
    {
        PlaySound(_audioClipRefsSo.footstep, position, volume);
    }

    public void PlayCountdownSound()
    {
        PlaySound(_audioClipRefsSo.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(_audioClipRefsSo.warning, position);
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        if (_volume > 1f)
        {
            _volume = 0;
        }

        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME_KEY, _volume);
        PlayerPrefs.Save();     // not strictly necessary, unless Unity crashes or something
    }
}
