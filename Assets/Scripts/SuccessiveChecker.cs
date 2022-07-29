using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccessiveChecker : MonoBehaviour
{
    [SerializeField] AudioClip splitSFX;
    [SerializeField] float pitchIncreaseValue;
    [SerializeField] float maxPitchValue;

    AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayPerfectSound()
    {
        _audioSource.pitch += pitchIncreaseValue;

        if (_audioSource.pitch > maxPitchValue)
        {
            _audioSource.pitch = maxPitchValue;
        }

        _audioSource.Play();
    }

    public void PlayNormalSound()
    {
        _audioSource.pitch = 1f;
        _audioSource.PlayOneShot(splitSFX);
    }

    public void ResetPitch()
    {
        _audioSource.pitch = 1f;
    }
}
