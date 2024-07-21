using System;
using System.Collections;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public event Action OnShoot;
    public Transform bulletSpawnPosition;

    private InputController _inputController;
    private BulletsPool _bulletsPool;
    [SerializeField] private Transform _cannon;

    [SerializeField] private float _minVerticalAngle = 40.0f;
    [SerializeField] private float _maxVerticalAngle = 90.0f;
    [SerializeField] private float _minHorizontalAngle = -30.0f;
    [SerializeField] private float _maxHorizontalAngle = 30.0f;

    [Header("Shooting settings")]
    [SerializeField] private float _power = 10.0f;
    [SerializeField] private float _maxPower = 50.0f;
    [SerializeField] private float _projectileSpeed = 10f;
    [SerializeField] private LayerMask _collisionLayers;

    [Header("Cannon recoil anim")]
    [SerializeField] private float _recoilDistance = 0.5f;
    [SerializeField] private float _recoilDuration = 0.1f;

    private float _horizontalAngle = 0.0f;
    private float _verticalAngle = 0.0f;
    private Vector3 _originalPosition;

    public float Power { get => _power; set => _power = Mathf.Clamp(value, 10, _maxPower); }

    private void Awake()
    {
        ServiceLocator.RegisterService(this);
    }

    private void Start()
    {
        _inputController = ServiceLocator.GetService<InputController>();
        _inputController.OnShoot += Shoot;

        _bulletsPool = ServiceLocator.GetService<BulletsPool>();

        _originalPosition = _cannon.localPosition;
    }

    private void Update()
    {
        Power += _inputController.MouseWheelScroll;

        _horizontalAngle += _inputController.MouseX;
        _verticalAngle -= _inputController.MouseY;

        _horizontalAngle = Mathf.Clamp(_horizontalAngle, _minHorizontalAngle, _maxHorizontalAngle);
        _verticalAngle = Mathf.Clamp(_verticalAngle, _minVerticalAngle, _maxVerticalAngle);

        transform.rotation = Quaternion.Euler(transform.rotation.x, _horizontalAngle, 0.0f);
        _cannon.localRotation = Quaternion.Euler(_verticalAngle, _cannon.localRotation.y, 0.0f);
    }

    private void Shoot()
    {
        StartCoroutine(RecoilAnim());

        var bullet = _bulletsPool.GetObject();
        if (bullet == null)
        {
            bullet = new GameObject("Bullet", typeof(CubeBulletMeshCreator), typeof(Bullet));
        }
        else
        {
            bullet.GetComponent<CubeBulletMeshCreator>().GenerateMesh();
        }
        bullet.transform.position = bulletSpawnPosition.position;
        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Shoot(_power, bulletSpawnPosition.up, _collisionLayers);

        OnShoot?.Invoke();
    }

    private IEnumerator RecoilAnim()
    {
        Vector3 recoilPosition = _originalPosition - _cannon.up * _recoilDistance;
        float elapsedTime = 0f;

        while (elapsedTime < _recoilDuration)
        {
            _cannon.localPosition = Vector3.Lerp(_originalPosition, recoilPosition, elapsedTime / _recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < _recoilDuration)
        {
            _cannon.localPosition = Vector3.Lerp(recoilPosition, _originalPosition, elapsedTime / _recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        _inputController.OnShoot -= Shoot;
    }
}