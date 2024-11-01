using TMPro;
using UnityEngine;

public class WaveController : Singleton<WaveController>
{

    [SerializeField] private TextMeshProUGUI WaveText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void UpdateWaveText(int waveCount)
    {
        WaveText.text = waveCount.ToString()+ " / 3";
    }

 
}
