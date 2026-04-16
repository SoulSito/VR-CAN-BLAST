using UnityEngine;
using UnityEngine.UI;

public class GunUIController : MonoBehaviour
{
    public Slider bulletsSlider;
    public Toggle laserToggle;

    [SerializeField] private ShootController shootController;

    private void Start()
    {
        bulletsSlider.onValueChanged.AddListener(OnBulletsChanged);

        laserToggle.onValueChanged.AddListener(OnLaserChanged);
    }

    void OnBulletsChanged(float value)
    {
        ApplySettings();
    }

    void OnLaserChanged(bool value)
    {
        ApplySettings();
    }

    public void ApplySettings()
    {
        if (shootController == null) return;

        GunSettings settings = new GunSettings(
            (int)bulletsSlider.value,
            laserToggle.isOn
        );

        shootController.Setup(settings);
    }
}