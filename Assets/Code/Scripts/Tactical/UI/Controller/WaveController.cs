using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WaveController : Singleton<WaveController>
{
    [SerializeField] private TextMeshProUGUI waveLabel;
    
    public void UpdateWaveText(int waveCount)
    {
        waveLabel.text = $"{waveCount} / {SpawnManager.Inst.maxWaveCount}";
    }
}
