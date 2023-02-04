using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private Light lightSource;
    private float startIntensity;
    
    private void Start()
    {
        lightSource = GetComponent<Light>();
        startIntensity = Random.value + 1f;
    }

    private void Update()
    {
        lightSource.intensity = Mathf.Sin(Time.timeSinceLevelLoad * startIntensity + startIntensity) / 4 + 0.5f;
    }
}
