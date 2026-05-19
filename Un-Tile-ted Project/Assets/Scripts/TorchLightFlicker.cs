using UnityEngine;

[RequireComponent(typeof(Light))]
public class TorchLightFlicker : MonoBehaviour
{
    private Light torchLight;

    [Header("Base Settings")]
    [SerializeField] private float baseIntensity = 10f;
    [SerializeField] private float baseRange = 85;

    [Header("Flicker Settings")]
    [SerializeField] private float intensityVariation = 0.3f;
    [SerializeField] private float rangeVariation = 0.5f;

    [SerializeField] private float flickerSpeed = 8f;

    private float randomOffset;

    void Start()
    {
        torchLight = GetComponent<Light>();

        randomOffset = Random.Range(0f, 100f);

        torchLight.intensity = baseIntensity;
        torchLight.range = baseRange;
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);

        torchLight.intensity =
            baseIntensity +
            (noise - 0.5f) * intensityVariation;

        torchLight.range =
            baseRange +
            (noise - 0.5f) * rangeVariation;
    }
}