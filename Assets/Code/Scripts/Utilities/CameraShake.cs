using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    public CinemachineImpulseSource cinemachinePlayerImpulseSource;

    public void Shake(float shakeAmount, bool isPlayer = false)
    {
        var impulseSourceRef = isPlayer ? cinemachinePlayerImpulseSource : cinemachineImpulseSource;
        impulseSourceRef.m_DefaultVelocity = Random.insideUnitSphere.normalized * (shakeAmount * (isPlayer ? 1.5f : 1));
        impulseSourceRef.GenerateImpulse();
    }
}
