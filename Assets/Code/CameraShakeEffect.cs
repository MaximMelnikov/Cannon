using System.Collections;
using UnityEngine;

public class CameraShakeEffect : MonoBehaviour
{
    private CannonController _cannonController;

    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeMagnitude = 0.5f;

    private void Start()
    {
        _cannonController = ServiceLocator.GetService<CannonController>();
        _cannonController.OnShoot += Shake;
    }

    private void Shake()
    {
        StartCoroutine(ShakeEffect());
    }

    private IEnumerator ShakeEffect()
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    private void OnDestroy()
    {
        _cannonController.OnShoot -= Shake;
    }
}