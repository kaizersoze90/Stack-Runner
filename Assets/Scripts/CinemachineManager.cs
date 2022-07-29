using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera followCam, danceCam;

    [Header("Camera Settings")]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;

    CinemachineBasicMultiChannelPerlin _shaker;

    void Start()
    {
        _shaker = followCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ReleaseFollowCam()
    {
        followCam.Follow = null;
    }

    public void SwitchCamera()
    {
        followCam.Priority = followCam.Priority == 1 ? 0 : 1;
        danceCam.Priority = danceCam.Priority == 0 ? 1 : 0;
    }

    public void ShakeCamera()
    {
        StartCoroutine(nameof(ProcessShakeEffect));
    }

    IEnumerator ProcessShakeEffect()
    {
        _shaker.m_AmplitudeGain = shakeIntensity;
        yield return new WaitForSeconds(shakeDuration);
        _shaker.m_AmplitudeGain = 0f;
    }
}
