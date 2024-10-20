using UnityEngine;

public static class TimeCaster
{
    public static void SetTimeScale(float value)
    {
        Time.timeScale = value;
        Time.fixedDeltaTime = value * 0.02f;
        Time.maximumDeltaTime = value * 0.2f;
    }
}
