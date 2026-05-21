using UnityEngine;
using UnityEngine.UI;

public class GunUIController : MonoBehaviour
{
    public Slider bulletsSlider;
    public Toggle laserToggle;

    private void Start()
    {
        bulletsSlider.onValueChanged.AddListener(OnBulletsChanged);
        laserToggle.onValueChanged.AddListener(OnLaserChanged);

        ApplySettings();
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
        if (GameSettingsManager.Instance == null) return;
        Debug.Log(bulletsSlider.value);
        Debug.Log(laserToggle.isOn);
        GameSettingsManager.Instance.currentGunSettings = new GunSettings( (int)bulletsSlider.value, laserToggle.isOn);
    }

}