using Cinemachine;
using UnityEngine;

public class CameraShake : Singleton<CameraShake>
{
    public CinemachineImpulseSource cinemachineImpulseSource;
    public CinemachineImpulseSource cinemachinePlayerImpulseSource;
    [SerializeField] private float shakeAmount;

    public void Shake(bool isPlayer = false)
    {
        var impulseSourceRef = isPlayer ? cinemachinePlayerImpulseSource : cinemachineImpulseSource;
        impulseSourceRef.m_DefaultVelocity = Random.insideUnitSphere.normalized * (shakeAmount * (isPlayer ? 2 : 1));
        impulseSourceRef.GenerateImpulse();
    }
}
