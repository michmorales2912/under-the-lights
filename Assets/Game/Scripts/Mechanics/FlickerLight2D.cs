using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Unity 6 URP

/// <summary>
/// Parpadeo de luces tipo neón defectuoso.
/// Requiere un componente Light2D en el mismo GameObject.
/// </summary>
[RequireComponent(typeof(Light2D))]
public class FlickerLight2D : MonoBehaviour
{
    [Header("Intensidad")]
    public float baseIntensity    = 0.8f;
    public float minIntensity     = 0.1f;
    public float maxIntensity     = 1.2f;

    [Header("Timing")]
    public float minInterval      = 0.08f;
    public float maxInterval      = 0.6f;

    [Header("Probabilidad de parpadeo")]
    [Range(0f, 1f)]
    public float flickerChance    = 0.35f;

    private Light2D _light;

    void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.intensity = baseIntensity;
    }

    void OnEnable()  => StartCoroutine(Flicker());
    void OnDisable() => StopAllCoroutines();

    IEnumerator Flicker()
    {
        while (true)
        {
            if (Random.value < flickerChance)
            {
                _light.intensity = Random.Range(minIntensity, maxIntensity);
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval * 0.3f));
                _light.intensity = baseIntensity;
            }
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }
}