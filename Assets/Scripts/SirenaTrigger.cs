using System.Collections;
using UnityEngine;

public class SirenaTrigger : MonoBehaviour
{
    [SerializeField] private EnterInHouseTrigger _enterInHouseTrigger;
    [SerializeField] private AudioSource _sirenaAudioSource;
    [SerializeField] private float _changeVolumeSpeed = 1f;

    private bool _isActivateSirena = false;
    private Coroutine _changeAudioVolumeCoroutine;

    private float _minAudioVolume = 0f;
    private float _maxAudioVolume = 1f;

    private void Start()
    {
        _sirenaAudioSource.volume = _minAudioVolume;

        _enterInHouseTrigger.OnHouseEnter += OnHouseEnter;
        _enterInHouseTrigger.OnHouseExit += OnHouseExit;
    }

    private void OnDestroy()
    {
        _enterInHouseTrigger.OnHouseEnter -= OnHouseEnter;
        _enterInHouseTrigger.OnHouseExit -= OnHouseExit;
    }

    private void OnHouseEnter(Player player)
    {        
        StartSirena();        
    }

    private void OnHouseExit(Player player)
    {  
        StopSirena();        
    }

    private void StartSirena()
    {
        if (_isActivateSirena)
        {
            return;
        }
        
        _isActivateSirena = true;

        StopChangeAudioVolumeCoroutine();
        _changeAudioVolumeCoroutine = StartCoroutine(ChangeAudioVolumeCoroutine(_maxAudioVolume));
    }

    private void StopSirena()
    {
        if (_isActivateSirena == false)
        {
            return;
        }

        _isActivateSirena = false;

        StopChangeAudioVolumeCoroutine();
        _changeAudioVolumeCoroutine = StartCoroutine(ChangeAudioVolumeCoroutine(_minAudioVolume));
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
        if (_sirenaAudioSource.isPlaying == false)
        {
            _sirenaAudioSource.Play();
        }

        float volumeEpsilon = 0.01f;

        while (Mathf.Abs(_sirenaAudioSource.volume - targetVolume) > volumeEpsilon)
        {            
            yield return null;
            _sirenaAudioSource.volume = Mathf.MoveTowards(_sirenaAudioSource.volume, targetVolume, _changeVolumeSpeed * Time.deltaTime);
        }

        _sirenaAudioSource.volume = targetVolume;

        if (_sirenaAudioSource.isPlaying && Mathf.Approximately(_minAudioVolume, targetVolume))
        {
            _sirenaAudioSource.Stop();
        }
    }
}
