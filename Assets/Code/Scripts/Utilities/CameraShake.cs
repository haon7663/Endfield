using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private float shakeAmount;

    public void Shake()
    {
        cinemachineImpulseSource.m_DefaultVelocity = Random.insideUnitSphere.normalized * shakeAmount;
        cinemachineImpulseSource.GenerateImpulse();
    }
}
