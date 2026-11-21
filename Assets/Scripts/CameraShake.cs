using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [Header("Default Shake Settings")]
    public float defaultDuration = 0.2f;
    public float defaultMagnitude = 0.15f;

    private Vector3 originalLocalPos;
    private Coroutine shakeCoroutine;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    public void Shake()
    {
        Shake(defaultDuration, defaultMagnitude);
    }

    public void Shake(float duration, float magnitude)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float damper = 1f - (elapsed / duration);
            Vector2 offset = Random.insideUnitCircle * (magnitude * damper);

            transform.localPosition = originalLocalPos + new Vector3(offset.x, offset.y, 0f);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPos;
        shakeCoroutine = null;
    }
}