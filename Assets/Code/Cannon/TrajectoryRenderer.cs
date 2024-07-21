using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryRenderer : MonoBehaviour
{
    private const float _gravity = 9.81f;
    private const int resolution = 30;
    private const float timeStep = 0.1f;

    private CannonController _cannonController;
    private LineRenderer _lineRenderer;

    [SerializeField] private Vector3 _initialVelocity;

    void Start()
    {
        _cannonController = ServiceLocator.GetService<CannonController>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        _initialVelocity = _cannonController.bulletSpawnPosition.up * _cannonController.Power;
        RenderTrajectory(_cannonController.bulletSpawnPosition.position, _initialVelocity, _gravity);
    }

    public void RenderTrajectory(Vector3 startPosition, Vector3 initialVelocity, float gravity)
    {
        _lineRenderer.positionCount = resolution;

        Vector3[] trajectoryPoints = new Vector3[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float time = i * timeStep;
            trajectoryPoints[i] = CalculatePositionAtTime(startPosition, initialVelocity, gravity, time);
        }

        _lineRenderer.SetPositions(trajectoryPoints);
    }

    private Vector3 CalculatePositionAtTime(Vector3 startPosition, Vector3 initialVelocity, float gravity, float time)
    {
        float x = startPosition.x + initialVelocity.x * time;
        float y = startPosition.y + initialVelocity.y * time - 0.5f * gravity * time * time;
        float z = startPosition.z + initialVelocity.z * time;
        return new Vector3(x, y, z);
    }
}