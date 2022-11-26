using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenaTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource sirenaAudioSource;
    [SerializeField] private float changeVolumeSpeed = 1f;

    private bool _isActivateSirena = false;
    private Coroutine _changeAudioVolumeCoroutine;

    private float minAudioVolume = 0f;
    private float maxAudioVolume = 1f;

    private void Start()
    {
        sirenaAudioSource.volume = minAudioVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivateSirena == false && other.tag.Contains(GameTags.Player))
        {
            StartSirena();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isActivateSirena && other.tag.Contains(GameTags.Player))
        {
            StopSirena();
        }
    }

    private void StartSirena()
    {
        _isActivateSirena = true;

        StopChangeAudioVolumeCoroutine();
        _changeAudioVolumeCoroutine = StartCoroutine(ChangeAudioVolumeCoroutine(maxAudioVolume));
    }

    private void StopSirena()
    {
        _isActivateSirena = false;

        StopChangeAudioVolumeCoroutine();
        _changeAudioVolumeCoroutine = StartCoroutine(ChangeAudioVolumeCoroutine(minAudioVolume));
    }

    private void StopChangeAudioVolumeCoroutine()
    {
        if (_changeAudioVolumeCoroutine != null)
        {
            StopCoroutine( _changeAudioVolumeCoroutine);
            _changeAudioVolumeCoroutine = null;
        }
    }

    private IEnumerator ChangeAudioVolumeCoroutine(float targetVolume)
    {
        if (sirenaAudioSource.isPlaying == false)
        {
            sirenaAudioSource.Play();
        }

        float volumeEpsilon = 0.01f;

        while (Mathf.Abs(sirenaAudioSource.volume - targetVolume) > volumeEpsilon)
        {            
            yield return null;
            sirenaAudioSource.volume = Mathf.MoveTowards(sirenaAudioSource.volume, targetVolume, changeVolumeSpeed * Time.deltaTime);
        }

        sirenaAudioSource.volume = targetVolume;

        if (sirenaAudioSource.isPlaying && Mathf.Approximately(minAudioVolume, targetVolume))
        {
            sirenaAudioSource.Stop();
        }
    }


}
