using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private CannonController _cannonController;
    [SerializeField] private Slider _powerSlider;

    private void Start()
    {
        _cannonController = ServiceLocator.GetService<CannonController>();
    }

    void Update()
    {
        _powerSlider.value = _cannonController.Power;
    }
}