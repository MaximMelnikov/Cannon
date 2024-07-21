using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletsPool _bulletsPool;

    private const float gravity = 9.81f;
    private const float bounceDampening = 0.5f;

    private Vector3 _velocity;
    private int _collisionCount = 0;
    private LayerMask _collisionLayers;

    private float _scaleDuration = 0.1f;
    private float _scaleMagnitude = 0.2f;

    private void Awake()
    {
        _bulletsPool = ServiceLocator.GetService<BulletsPool>();
    }

    public void Shoot(float force, Vector3 direction, LayerMask collisionLayers)
    {
        _collisionCount = 0;
        _collisionLayers = collisionLayers;
        _velocity = direction.normalized * force;
    }

    private void Update()
    {
        _velocity.y -= gravity * Time.deltaTime;

        Vector3 newPosition = transform.position + _velocity * Time.deltaTime;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, _velocity, out hit, _velocity.magnitude * Time.deltaTime, _collisionLayers))
        {
            _collisionCount++;

            if (_collisionCount >= 2)
            {
                DestroyBullet();
            }
            else
            {
                StartCoroutine(CollisionEffect());
                _velocity = Vector3.Reflect(_velocity, hit.normal) * bounceDampening;
            }

            var decalComponent = hit.transform.GetComponent<DecalDrawer>();
            if (decalComponent)
            {
                decalComponent.DrawDecal(hit);
            }
        }
        else
        {
            transform.position = newPosition;
        }
    }

    void DestroyBullet()
    {
        StartCoroutine(CollisionEffect());

        _bulletsPool.ReturnObject(gameObject);
        var particlesPrefab = (GameObject) Resources.Load("Explosion", typeof(GameObject));
        Instantiate(particlesPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator CollisionEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _scaleDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * (1 - _scaleMagnitude), elapsedTime / _scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _scaleDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, elapsedTime / _scaleDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}