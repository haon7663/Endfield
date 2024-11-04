using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private float shakeAmount;

    public void Shake(float shakeAmountMultiplier = 1)
    {
        cinemachineImpulseSource.m_DefaultVelocity = Random.insideUnitSphere.normalized * (shakeAmount * shakeAmountMultiplier);
        cinemachineImpulseSource.GenerateImpulse();
    }
}
