using System.Collections;
using UnityEngine;

public class SirenaTrigger : MonoBehaviour
{
    [SerializeField] private HouseEnterDetector _houseEnterTrigger;
    [SerializeField] private HouseExitDetector _houseExitTrigger;
    [SerializeField] private AudioSource _sirenaAudioSource;
    [SerializeField] private float _changeVolumeSpeed = 1f;

    private bool _isActivateSirena = false;
    private Coroutine _changeAudioVolumeCoroutine;

    private float _minAudioVolume = 0f;
    private float _maxAudioVolume = 1f;

    private void Start()
    {
        _sirenaAudioSource.volume = _minAudioVolume;

        _houseEnterTrigger.HouseEnter += OnHouseEnter;
        _houseExitTrigger.HouseExit += OnHouseExit;
    }

    private void OnDestroy()
    {
        _houseEnterTrigger.HouseEnter -= OnHouseEnter;
        _houseExitTrigger.HouseExit -= OnHouseExit;
    }    

    private void OnHouseEnter(Player player)
    {
        if (_isActivateSirena)
        {
            return;
        }
        
        _isActivateSirena = true;
        ChangeAudioVolume(_maxAudioVolume);
    }

    private void OnHouseExit(Player player)
    {
        if (_isActivateSirena == false)
        {
            return;
        }

        _isActivateSirena = false;
        ChangeAudioVolume(_minAudioVolume);
    }

    private void ChangeAudioVolume(float targetAudioVolume)
    {
        StopChangeAudioVolumeCoroutine();
        _changeAudioVolumeCoroutine = StartCoroutine(ChangeAudioVolumeCoroutine(targetAudioVolume));
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

        while (_sirenaAudioSource.volume != targetVolume)
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
